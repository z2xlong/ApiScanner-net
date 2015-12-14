using System;
using Microsoft.Cci;
using System.Collections.Generic;

namespace ApiScanner.Core
{
    public class ApiVisibility : ICompatible<ApiVisibility>
    {
        #region Static members

        static Dictionary<TypeMemberVisibility, ApiVisibilityEnum> _vmapping = InitMapping();

        static Dictionary<TypeMemberVisibility, ApiVisibilityEnum> InitMapping()
        {
            Dictionary<TypeMemberVisibility, ApiVisibilityEnum> mapping = new Dictionary<TypeMemberVisibility, ApiVisibilityEnum>();
            mapping.Add(TypeMemberVisibility.Assembly, ApiVisibilityEnum.Assembly);
            mapping.Add(TypeMemberVisibility.Family, ApiVisibilityEnum.Family);
            mapping.Add(TypeMemberVisibility.FamilyAndAssembly, ApiVisibilityEnum.FamilyAndAssembly);
            mapping.Add(TypeMemberVisibility.FamilyOrAssembly, ApiVisibilityEnum.FamilyOrAssembly);
            mapping.Add(TypeMemberVisibility.Private, ApiVisibilityEnum.Private);
            mapping.Add(TypeMemberVisibility.Public, ApiVisibilityEnum.Public);

            return mapping;
        }

        static ApiVisibilityEnum Extract(TypeMemberVisibility cciVisibility)
        {
            ApiVisibilityEnum visibility;
            if (!_vmapping.TryGetValue(cciVisibility, out visibility))
                visibility = ApiVisibilityEnum.Default;

            return visibility;
        }

        #endregion

        public ApiVisibilityEnum Visibility { get; private set; }

        public ApiVisibility(TypeMemberVisibility cciVisibility)
        {
            this.Visibility = Extract(cciVisibility);
        }

        public Compatibility IsCompatible(ApiVisibility old)
        {
            ChangeLevel level = ChangeLevel.NoChange;

            if (this.Visibility < old.Visibility)
                level = ChangeLevel.Broken;
            else if (this.Visibility > old.Visibility)
                level = ChangeLevel.Compatible;

            return new Compatibility(level);
        }

        public override string ToString()
        {
            return this.Visibility.ToString();
        }
    }
}
