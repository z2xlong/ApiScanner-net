using System.Collections.Generic;
using Microsoft.Cci;

namespace ApiCore
{
    public class Api : ICompatible<Api>
    {
        Dictionary<string, string> _genericConstraints = new Dictionary<string, string>();

        public ApiKind MemberKind { get; private set; }

        public string Signature { get; private set; }

        internal Api(ITypeDefinitionMember member)
        {
            this.MemberKind = ApiHelper.CheckMemberKind(member);
            this.Signature = ApiHelper.GetSignature(member);

            var method = member as IMethodDefinition;
            if (member != null)
            {
                foreach (var genpar in method.GenericParameters)
                    _genericConstraints.Add(genpar.Name.Value, ApiHelper.PrintConstraints(genpar));
            }

        }

        public override string ToString()
        {
            return this.Signature;
        }

        public bool IsCompatible(Api old, IList<string> incompatibility)
        {
            bool result = true;
            if (this.Signature != old.Signature || this.MemberKind != old.MemberKind)
            {
                incompatibility.Add(this.Signature);
                result = false;
            }

            foreach (var genpar in old._genericConstraints)
            {
                if (_genericConstraints[genpar.Key] != genpar.Value)
                {
                    incompatibility.Add(string.Format("{0} {1} where : {2} is changed.", this.Signature, genpar.Key, genpar.Value));
                    result = false;
                }
            }

            return result;
        }

    }
}
