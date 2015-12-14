using System.Collections.Generic;
using Microsoft.Cci;

namespace ApiScanner.Core
{
    public class TypeMemberApi : ICompatible<TypeMemberApi>
    {
        Dictionary<string, string> _genericConstraints = new Dictionary<string, string>();

        public ApiKind MemberKind { get; private set; }

        public string Signature { get; private set; }

        internal TypeMemberApi(ITypeDefinitionMember member)
        {
            this.MemberKind = ApiHelper.CheckMemberKind(member);
            this.Signature = ApiHelper.GetSignature(member);

            var mt = member as IMethodDefinition;
            if (mt != null)
            {
                foreach (var genpar in mt.GenericParameters)
                    _genericConstraints.Add(genpar.Name.Value, ApiHelper.PrintConstraints(genpar));
            }

        }

        public override string ToString()
        {
            return this.Signature;
        }

        public Compatibility IsCompatible(TypeMemberApi old)
        {
            int result = 0;
            if (this.Signature != old.Signature || this.MemberKind != old.MemberKind)
            {
                //incompatibility.Add(this.Signature);
                result = -1;
            }

            foreach (var genpar in old._genericConstraints)
            {
                if (_genericConstraints[genpar.Key] != genpar.Value)
                {
                    //incompatibility.Add(string.Format("{0} {1} where : {2} is changed.", this.Signature, genpar.Key, genpar.Value));
                    result = -1;
                }
            }

            return result;
        }

    }
}
