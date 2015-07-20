using Microsoft.Cci;
using Microsoft.Cci.MutableContracts;
using System;
using System.Collections.Generic;

namespace ApiCore
{
    public class Scanner
    {
        public static AssemblySketch Traverse(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Assembly Path should not be empty.");

            using (MetadataReaderHost host = new PeReader.DefaultHost())
            {
                IModule module = host.LoadUnitFrom(path) as IModule;
                if (module == null || module is Dummy)
                {
                    Environment.Exit(1);
                    throw new ArgumentException("'{0}' is not a PE file containing a CLR module or assembly.", path);
                }
                var t = new ApiTraverser(host, module.ContainingAssembly);
                t.Traverse(module);
                return t.Assembly;
            }
        }

        public static IEnumerable<string> CompareApi(AssemblySketch oldVersion, AssemblySketch newVersion)
        {
            List<string> incompatibility = new List<string>();
            if (newVersion.IsCompatible(oldVersion, incompatibility))
                return null;
            else
                return incompatibility;
        }

    }
}
