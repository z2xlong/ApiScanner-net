using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DummyAssembly;

namespace DummyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Child c = new Child()
            {
                Name = "John Doe",
                Mobile = "188",
                Email = "John@ctrip.com"
            };

            Console.WriteLine(Enum1.Item1 | Enum1.Item2);
            Console.WriteLine((new Class1<string>()).GetContactInfo(c));
            Console.WriteLine((new Class2.Class3()).Name);
            Console.ReadLine();
        }
    }
}
