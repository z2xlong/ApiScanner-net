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
        const int tmp = 5;

        static void Main(string[] args)
        {
            Child c = new Child()
            {
                Name = "John Doe",
                Mobile = "188",
                Email = "John@ctrip.com",
                SkinColor = Color.Yellow
            };

            Console.WriteLine(c.GetContactInfo());

            Standalone<string> s = new Standalone<string>("ss");
            Console.WriteLine(s.Name);

            //Console.WriteLine((new Class1<string>()).GetContactInfo(c));
            //Console.WriteLine((new Class2.Class3()).Name);
            new InterfaceImp().Execute(c.Name);

            var imp = new AbstractImp();
            imp.Do();

            Console.ReadLine();
        }
    }
}
