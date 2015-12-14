using Microsoft.Cci;
using System;
using System.Collections.Generic;

namespace ApiScanner.Core
{
    //public class ApiModifier : ICompatible<ApiModifier>
    //{
    //    HashSet<string> _modifiers = new HashSet<string>();

    //    public ApiModifier(IMethodDefinition method)
    //    {
    //        if (method.IsStatic) _modifiers.Add("static");
    //        if (method.ResolvedMethod.IsAbstract) _modifiers.Add("abstract");
    //        if (method.ResolvedMethod.IsExternal) _modifiers.Add("external");
    //        if (method.ResolvedMethod.IsNewSlot) _modifiers.Add("new");
    //        if (method.ResolvedMethod.IsSealed) _modifiers.Add("sealed");
    //        if (method.ResolvedMethod.IsVirtual) _modifiers.Add("virtual");
    //    }

    //    public ApiModifier(IFieldDefinition field)
    //    {
    //        if (field.Type.TypeCode == PrimitiveTypeCode.Pointer) _modifiers.Add("unsafe");
    //        if (field.IsCompileTimeConstant) _modifiers.Add("const");
    //        if (field.IsStatic) _modifiers.Add("static");
    //        if (field.IsReadOnly) _modifiers.Add("readonly");
    //        if (MemberHelper.IsVolatile(field)) _modifiers.Add("volatile");
    //    }

    //    public ApiModifier(ITypeDefinition type)
    //    {
    //        if (type.IsStatic)
    //        {
    //            _modifiers.Add("static");
    //        }
    //        else
    //        {
    //            if (type.IsAbstract && !type.IsInterface) _modifiers.Add("abstract");
    //            if (type.IsSealed && !type.IsValueType) _modifiers.Add("sealed");
    //        }
    //    }

    //    //public ApiModifier(IPropertyDefinition property)
    //    //{
    //    //    property.ReturnValueIsByRef
    //    //}

    //    //public Compatibility IsCompatible(ApiModifier old)
    //    //{
    //    //    if (old.Modifier != this.Modifier)
    //    //        return Compatibility.Broken;
    //    //    else
    //    //        return Compatibility.NoChange;
    //    //}

    //}

}
