using System;
using System.Collections.Generic;
using Microsoft.Cci;
using System.Text;

namespace ApiScanner.Core
{
    public class TypeApi : ICompatible<TypeApi>
    {
        public string Signature { get; private set; }

        public ApiParent Parent { get; private set; }

        public ApiGeneric Generic { get; private set; }

        public ApiChildren Children { get; private set; }

        internal TypeApi(ITypeDefinition definition)
        {
            this.Signature = ApiHelper.GetSignature(definition);
            this.Generic = new ApiGeneric(definition);
            this.Parent = new ApiParent(definition);
            this.Children = new ApiChildren();
        }

        internal bool EnrollApi(ITypeDefinitionMember member)
        {
            TypeMemberApi api = new TypeMemberApi(member);
            return this.Children.Enroll(api);
        }

        public Compatibility IsCompatible(TypeApi old)
        {
            int result = 0;
            if (this.Signature != old.Signature)
            {
                //incompatibility.Add(this.Signature);
                result = -1;
            }

            //result &= this.Parent.IsCompatible(old.Parent);
            //result &= this.Generic.IsCompatible(old.Generic);
            //result &= this.Children.IsCompatible(old.Children);

            return result;
        }
    }
}
