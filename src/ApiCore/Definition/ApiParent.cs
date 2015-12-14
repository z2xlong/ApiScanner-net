using Microsoft.Cci;
using System.Collections.Generic;
using System.Text;

namespace ApiScanner.Core
{
    public class ApiParent : ICompatible<ApiParent>
    {
        HashSet<string> _parents = new HashSet<string>();

        public ApiParent(ITypeDefinition type)
        {
            foreach (var parent in type.BaseClasses)
                _parents.Add(ApiHelper.GetSignature(parent.ResolvedType));

        }

        public Compatibility IsCompatible(ApiParent old)
        {
            ChangeLevel level = ChangeLevel.NoChange;
            StringBuilder sb = new StringBuilder();

            foreach (var parent in old._parents)
            {
                if (!_parents.Contains(parent))
                {
                    sb.Append(string.Format("Base class {0} is changed.", parent));
                    level = ChangeLevel.Broken;
                }
            }

            return new Compatibility(level, sb.ToString());
        }
    }
}
