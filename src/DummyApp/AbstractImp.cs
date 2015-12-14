using System;
using DummyAssembly;

namespace DummyApp
{
    class AbstractImp : AbstractBase
    {
        public override void Do()
        {
            Console.WriteLine("Implement AbstractBase Do method.");
        }
    }
}
