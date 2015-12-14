using System;

namespace DummyAssembly
{
    public interface Interface1<M> where M : class
    {
        void Execute(M obj);

        void DoSomething();
    }
}
