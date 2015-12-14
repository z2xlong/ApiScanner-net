using Microsoft.Cci;
using System.Collections.Generic;
using System.Text;

namespace ApiScanner.Core
{
    public class ApiGeneric : ICompatible<ApiGeneric>
    {
        Dictionary<string, string> _genericConstraints = new Dictionary<string, string>();

        public ApiGeneric(ITypeDefinition type)
        {
            foreach (var genpar in type.GenericParameters)
                _genericConstraints.Add(genpar.Name.Value, ApiHelper.PrintConstraints(genpar));
        }

        public Compatibility IsCompatible(ApiGeneric old)
        {
            ChangeLevel level = ChangeLevel.NoChange;
            StringBuilder sb = new StringBuilder();

            foreach (var genpar in old._genericConstraints)
            {
                if (_genericConstraints[genpar.Key] != genpar.Value)
                {
                    sb.Append(string.Format("{0} where : {1} is changed.", genpar.Key, genpar.Value));
                    level = ChangeLevel.Broken;
                }
            }

            return new Compatibility(level, sb.ToString());
        }
    }
}
