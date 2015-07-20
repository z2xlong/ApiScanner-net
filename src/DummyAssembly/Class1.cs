using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyAssembly
{
    public class Class1<T> where T : class
    {
        public event EventHandler<string> Errors;
        public delegate bool TryParseConverter<in TInput, TOutput>(TInput input, out TOutput output);


        private bool ClassOnePrivateAct()
        {
            return false;
        }

        public char[] GetContactInfo(Child child)
        {
            return child.GetContactInfo().ToCharArray();
        }
    }
}
