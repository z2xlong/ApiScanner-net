using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cci;
using System.Reflection;

namespace ApiScanner.Core
{
    internal sealed class ApiHelper
    {
        #region MyRegion

        static readonly Dictionary<Type, ApiKind> _kindMapping = InitKindMapping();

        private static Dictionary<Type, ApiKind> InitKindMapping()
        {
            var mapping = new Dictionary<Type, ApiKind>();
            mapping.Add(typeof(IMethodDefinition), ApiKind.Method);
            mapping.Add(typeof(IFieldDefinition), ApiKind.Field);
            mapping.Add(typeof(IPropertyDefinition), ApiKind.Property);
            mapping.Add(typeof(IGlobalFieldDefinition), ApiKind.GlobalField);
            mapping.Add(typeof(IGlobalMethodDefinition), ApiKind.GlobalMethod);
            mapping.Add(typeof(IEventDefinition), ApiKind.Event);
            mapping.Add(typeof(INestedTypeDefinition), ApiKind.NestedType);

            return mapping;
        }

        public static ApiKind CheckMemberKind(ITypeDefinitionMember member)
        {
            ApiKind kind;
            if (_kindMapping.TryGetValue(member.GetType(), out kind))
                return kind;

            return ApiKind.Other;
        }

        #endregion

        #region Signature

        public static string GetSignature<T>(T member) where T : ITypeDefinitionMember
        {
            if (member is IEventDefinition)
                return GetSignature((IEventDefinition)member);
            if (member is IFieldDefinition)
                return GetSignature((IFieldDefinition)member);
            if (member is IPropertyDefinition)
                return GetSignature((IPropertyDefinition)member);
            if (member is IMethodDefinition)
                return GetSignature((IMethodDefinition)member);

            return member.Name.Value;
        }

        public static string GetSignature(IEventDefinition eventDefinition)
        {
            return string.Format("{0} {1} {2}", GetVisibility(eventDefinition), eventDefinition.Type, MemberHelper.GetMemberSignature(eventDefinition, GetBaseFormatOption() | NameFormattingOptions.MemberKind));
        }

        public static string GetSignature(IFieldDefinition field)
        {
            return string.Format("{0} {1} {2}", field.Visibility, field.Type, MemberHelper.GetMemberSignature(field, GetBaseFormatOption()));
        }

        public static string GetSignature(ITypeDefinition typeDefinition)
        {
            return string.Format("public {0} {1}", GetApiType(typeDefinition), TypeHelper.GetTypeName(typeDefinition, GetBaseFormatOption()));
        }

        public static string GetSignature(IPropertyDefinition property)
        {
            string getter = property.Getter == null ? string.Empty : string.Format("{0} get;", ((IMethodDefinition)property.Getter).Visibility);
            string setter = property.Setter == null ? string.Empty : string.Format("{0} set;", ((IMethodDefinition)property.Setter).Visibility);

            return string.Format("{0} {1} {2}{{{3} {4}}}", GetVisibility(property), property.Type, MemberHelper.GetMemberSignature(property, GetBaseFormatOption()), getter, setter);

        }

        public static string GetSignature(IMethodDefinition methodDefinition)
        {
            string sig = MemberHelper.GetMethodSignature(methodDefinition, NameFormattingOptions.Signature | NameFormattingOptions.ReturnType | NameFormattingOptions.Modifiers | NameFormattingOptions.TypeParameters);
            return string.Format("{0} {1}", GetVisibility(methodDefinition), sig);
        }

        public static string PrintConstraints(IGenericParameter genpar)
        {
            bool first = true;
            StringBuilder cstr = new StringBuilder();
            if (genpar.MustBeReferenceType)
            {
                cstr.Append("class");
                first = false;
            }
            if (genpar.MustBeValueType)
            {
                cstr.Append("struct");
                first = false;
            }
            foreach (var c in genpar.Constraints)
            {
                if (TypeHelper.TypesAreEquivalent(c, c.PlatformType.SystemValueType))
                    continue;

                if (first)
                    first = false;
                else
                    cstr.Append(",");

                cstr.Append(GetSignature(c.ResolvedType));
            }
            if (genpar.MustHaveDefaultConstructor && !genpar.MustBeValueType)
            {
                if (first)
                    first = false;
                else
                    cstr.Append(",");

                cstr.Append("new ()");
            }

            return cstr.ToString();
        }

        public static string GetSignature(IAssemblyReference assembly)
        {
            return string.Format("Name={0},Version={1},Culture={2},PublicKeyToken={3}",
                assembly.Name, assembly.Version, assembly.Culture, ComputePublicKey(assembly.PublicKeyToken));
        }

        public static string ComputePublicKey(IEnumerable<byte> token)
        {
            byte[] bytes = (new List<byte>(token)).ToArray();
            return BitConverter.ToString(bytes);
        }

        #endregion

        #region help mts

        static NameFormattingOptions GetBaseFormatOption()
        {
            return NameFormattingOptions.TypeParameters | NameFormattingOptions.Modifiers | NameFormattingOptions.Signature;
        }

        static string GetApiType(ITypeDefinition type)
        {
            string apiType = "Other";

            if (type.IsInterface)
                apiType = "Interface";
            else if (type.IsEnum)
                apiType = "Enum";
            else if (type.IsClass)
                apiType = "Class";
            else if (type.IsDelegate)
                apiType = "Delegate";

            return apiType;
        }

        static string GetVisibility(INestedTypeDefinition nestedType)
        {
            switch (nestedType.Visibility)
            {
                case TypeMemberVisibility.Assembly: return "internal";
                case TypeMemberVisibility.Family: return "protected";
                case TypeMemberVisibility.FamilyAndAssembly: return "protected and internal";
                case TypeMemberVisibility.FamilyOrAssembly: return "protected internal";
                case TypeMemberVisibility.Public: return "public";
                default: return "private";
            }
        }

        static string GetVisibility(ITypeDefinitionMember typeDefinitionMember)
        {
            switch (typeDefinitionMember.Visibility)
            {
                case TypeMemberVisibility.Assembly: return "internal";
                case TypeMemberVisibility.Family: return "protected";
                case TypeMemberVisibility.FamilyAndAssembly: return "protected and internal";
                case TypeMemberVisibility.FamilyOrAssembly: return "protected internal";
                case TypeMemberVisibility.Public: return "public";
                default: return "private";
            }
        }

        public static string GetEnumValue(Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            string valueStr = value.ToString();
            FieldInfo fieldInfo = type.GetField(valueStr);

            // Get the stringvalue attributes
            EnumValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(EnumValueAttribute), false) as EnumValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Value : valueStr;
        }

        #endregion
    }
}
