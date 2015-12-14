using System;
using DummyAssembly;

namespace DummyApp
{
    class InterfaceImp : Interface1<string>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void DoSomething()
        {
            throw new NotImplementedException();
        }

        public void Execute(string obj)
        {
            //throw new NotImplementedException();
            Console.WriteLine(obj);
        }
    }
}
