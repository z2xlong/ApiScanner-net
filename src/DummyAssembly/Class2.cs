using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyAssembly
{
    public class Class2 : IDisposable
    {
        public string this[int i]
        {
            get { return string.Empty; }
        }

        public void ClassTwoAct()
        {
            return;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return i.ToString();
        }

        //public string GetString(bool b)
        //{
        //    return b ? "true" : "false";
        //}

        //public void GenericMethod<T>(T obj) where T : class
        //{
        //    return;
        //}

        public class Class3
        {
            public string Name { get { return "Nested Class3's Name."; } }

            public class Class1
            {

            }
        }
    }
}
