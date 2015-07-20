using System;
using System.IO;
using System.Diagnostics;
using ApiCore;

namespace ConsoleScanner
{
    class Program
    {
        static string _path, _newVersion;

        static int Main(string[] args)
        {
#if DEBUG
            args = new string[4];
            args[0] = "-a";
            args[1] = @"Y:\Career\Ctrip2014\Project\ApiScannerTemp\DummyAssembly1\DummyAssembly.dll";
            args[2] = "-n";
            args[3] = @"Y:\Career\Ctrip2014\Project\ApiScannerTemp\DummyAssembly2\DummyAssembly.dll";
#endif
            if (!TryReadArgs(args))
            {
                WriteHelp();
#if !DEBUG
                return 0;
#endif
            }

            int result = 0;
            AssemblySketch assembly = Scanner.Traverse(_path);
            if (!string.IsNullOrWhiteSpace(_newVersion))
            {
                AssemblySketch newAssembly = Scanner.Traverse(_newVersion);
                var incoms = Scanner.CompareApi(assembly, newAssembly);
                if (incoms != null)
                {
                    foreach (string incom in incoms)
                        Console.WriteLine(incom);
                    result = -1;
                }
            }
            else
                PrintApi(assembly);

            if (result == 0)
                Console.WriteLine("Assemblies are compatible.");

#if DEBUG
            Console.ReadLine();
#endif
            return result;
        }

        static void PrintApi(AssemblySketch assembly)
        {
            Console.WriteLine(assembly.ToString());

            Console.WriteLine("References:");
            foreach (var refe in assembly.References)
                Console.WriteLine("\t{0}", refe.ToString());

            foreach (var type in assembly.Types)
            {
                Console.WriteLine(type.Signature);
                Console.WriteLine("\tBase Classes:");
                foreach (var parent in type.Parents)
                    Console.WriteLine("\t\t{0}", parent);
                foreach (var api in type.Apis)
                    Console.WriteLine("\t{0}", api.Signature);
            }
        }

        /// <summary>
        /// Read args
        /// </summary>
        /// <param name="args"></param>
        static bool TryReadArgs(string[] args)
        {
            Func<string, bool> readArg = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (readArg != null)
                {
                    if (readArg(args[i]))
                    {
                        readArg = null;
                        continue;
                    }
                    else
                        return false;
                }
                else if (!IsToken(args[i]) || !TryGetFunc(args[i], out readArg))
                    return false;
            }

            return readArg == null;
        }

        private static bool IsToken(string arg)
        {
            return !string.IsNullOrWhiteSpace(arg) && (arg[0] == '-' || arg[0] == '/');
        }

        private static bool TryGetFunc(string arg, out Func<string, bool> func)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(arg), "arg should not be null or empty.");

            switch (arg.Remove(0, 1))
            {
                case "a":
                    func = ReadAssemblyArg;
                    break;
                case "n":
                    func = ReadNewVersionArg;
                    break;
                case "h":
                    func = null;
                    break;
                default:
                    func = null;
                    return false;
            }

            return true;
        }

        static bool ReadAssemblyArg(string arg)
        {
            bool result = File.Exists(arg);

            if (result)
                _path = arg;
            else
                Console.WriteLine("Assembly file is not exists.");

            return result;
        }

        static bool ReadNewVersionArg(string arg)
        {
            bool result = File.Exists(arg);

            if (result)
                _newVersion = arg;
            else
                Console.WriteLine("The new version Assebmly file to be compared is not exists.");

            return result;
        }

        static void WriteHelp()
        {
            Console.WriteLine("Please input parameters as follows:\r\n\r\n\t-a: Assembly file to be scanned (nessary).\r\n\t-n: New version Assembly file to be compared (optional).\r\n\t-h: help info.\r\n");
        }

    }
}
