using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyAssembly
{
    public class Standalone<T> where T : class
    {
        public string Name;
        public event EventHandler<string> Errors;
        public delegate bool TryParseConverter<in TInput, TOutput>(TInput input, out TOutput output);

        public Standalone(string n)
        {
            Name = n;
        }

        public bool ClassOnePrivateAct()
        {
            return false;
        }
    }
}
