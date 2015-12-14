using System;
using System.Collections.Generic;
using Microsoft.Cci;
using Microsoft.Cci.Contracts;
using Microsoft.Cci.MutableContracts;
using System.Diagnostics;

namespace ApiScanner.Core
{
    public sealed class ApiTraverser : MetadataTraverser
    {
        readonly IMetadataReaderHost _host;
        readonly AssemblyApi _assembly;

        public AssemblyApi Assembly
        {
            get { return _assembly; }
        }

        public ApiTraverser(IMetadataReaderHost host, IAssembly assembly)
        {
            if (host == null)
                throw new ArgumentNullException("ContractAwareHost arg should not be null.");

            _host = host;
            _assembly = new AssemblyApi(assembly);
        }

        #region Traverserse

        public override void TraverseChildren(IEventDefinition eventDefinition)
        {
            if (!MemberHelper.IsVisibleOutsideAssembly(eventDefinition))
                return;

            _assembly.EnrollApi(eventDefinition);
        }

        public override void TraverseChildren(IFieldDefinition field)
        {
            if (!MemberHelper.IsVisibleOutsideAssembly(field))
                return;

            _assembly.EnrollApi(field);
        }

        public override void TraverseChildren(ITypeDefinition typeDefinition)
        {
            if (IsCompilerGenerated(typeDefinition))
                return;
            if (ContractHelper.IsContractClass(_host, typeDefinition))
                return;
            if (typeDefinition.GetType().Name == "GenericTypeParameter")
                return;
            if (!TypeHelper.IsVisibleOutsideAssembly(typeDefinition))
                return;

            _assembly.EnrollType(typeDefinition);

            base.TraverseChildren(typeDefinition);
        }

        public override void TraverseChildren(INestedTypeDefinition nestedTypeDefinition)
        {
            if (IsCompilerGenerated(nestedTypeDefinition))
                return;
            if (ContractHelper.IsContractClass(_host, nestedTypeDefinition))
                return;
            if (nestedTypeDefinition.GetType().Name == "GenericTypeParameter")
                return;
            if (!MemberHelper.IsVisibleOutsideAssembly(nestedTypeDefinition))
                return;

            base.TraverseChildren(nestedTypeDefinition);
        }

        public override void TraverseChildren(IPropertyDefinition property)
        {
            if (!MemberHelper.IsVisibleOutsideAssembly(property))
                return;

            _assembly.EnrollApi(property);
        }

        public override void TraverseChildren(IMethodDefinition methodDefinition)
        {
            if (IsCompilerGenerated(methodDefinition))
                return;
            if (IsGetter(methodDefinition) || IsSetter(methodDefinition))
                return;
            if (!MemberHelper.IsVisibleOutsideAssembly(methodDefinition))
                return;

            _assembly.EnrollApi(methodDefinition);
        }

        public override void TraverseChildren(IGlobalFieldDefinition globalField)
        {
            if (!MemberHelper.IsVisibleOutsideAssembly(globalField))
                return;
            _assembly.EnrollApi(globalField);
        }

        public override void TraverseChildren(IGlobalMethodDefinition globalMethod)
        {
            if (!MemberHelper.IsVisibleOutsideAssembly(globalMethod))
                return;
            _assembly.EnrollApi(globalMethod);
        }

        #endregion Visitors

        #region Helper methods

        private bool IsGetter(IMethodDefinition methodDefinition)
        {
            return methodDefinition.IsSpecialName && methodDefinition.Name.Value.StartsWith("get_");
        }

        private bool IsSetter(IMethodDefinition methodDefinition)
        {
            return methodDefinition.IsSpecialName && methodDefinition.Name.Value.StartsWith("set_");
        }

        private bool IsCompilerGenerated(ITypeDefinition typeDefinition)
        {
            return AttributeHelper.Contains(typeDefinition.Attributes, _host.PlatformType.SystemRuntimeCompilerServicesCompilerGeneratedAttribute);
        }

        private bool IsCompilerGenerated(IMethodDefinition methodDefinition)
        {
            return AttributeHelper.Contains(methodDefinition.Attributes, _host.PlatformType.SystemRuntimeCompilerServicesCompilerGeneratedAttribute);
        }

        #endregion

    }
}
