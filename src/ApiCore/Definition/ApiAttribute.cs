using System;
using System.Collections.Generic;
using Microsoft.Cci;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiScanner.Core.Definition
{
    public class ApiAttribute : ICompatible<ApiAttribute>
    {
        HashSet<string> _attributes = new HashSet<string>();

        public ApiAttribute(ITypeDefinition type)
        {
            foreach (var attr in type.Attributes)
                _attributes.Add(attr.ToString());
        }

        public Compatibility IsCompatible(ApiAttribute old)
        {
            throw new NotImplementedException();
        }
    }
}
