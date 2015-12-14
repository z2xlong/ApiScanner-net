using System;
using System.Collections.Generic;
using Microsoft.Cci;
using System.Diagnostics;
using System.Text;

namespace ApiScanner.Core
{
    public class AssemblyApi : ICompatible<AssemblyApi>
    {
        #region fileds & Properties

        string _sig = null;
        readonly HashSet<string> _references = new HashSet<string>();
        readonly Dictionary<string, TypeApi> _types = new Dictionary<string, TypeApi>();

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public string Culture { get; private set; }

        public string PublicKeyToken { get; private set; }

        public string Location { get; private set; }

        public IEnumerable<string> References
        {
            get { return _references; }
        }

        public IEnumerable<TypeApi> Types
        {
            get { return _types.Values; }
        }

        #endregion

        #region ctor

        internal AssemblyApi(IAssembly assembly)
        {
            Debug.Assert(assembly != null, "AssemblySkethc could not initialized from null IAssembly.");

            this.Location = assembly.Location;
            this.Name = assembly.Name.Value;
            this.Version = assembly.Version;
            this.PublicKeyToken = ApiHelper.ComputePublicKey(assembly.PublicKeyToken);

            foreach (IAssemblyReference reference in assembly.AssemblyReferences)
                _references.Add(ApiHelper.GetSignature(reference));
        }

        #endregion

        public override string ToString()
        {
            if (_sig == null)
                _sig = string.Format("Name={0},Version={1},Cluture={2},PublicKeyToken={3}",
                this.Name, this.Version, this.Culture, this.PublicKeyToken);
            return _sig;
        }

        internal void EnrollType(ITypeDefinition typeDefinition)
        {
            TypeApi type = new TypeApi(typeDefinition);
            if (!_types.ContainsKey(type.Signature))
            {
                _types[type.Signature] = type;
            }
        }

        internal bool EnrollApi(ITypeDefinitionMember member)
        {
            var csig = ApiHelper.GetSignature(member.ContainingTypeDefinition);
            TypeApi type;
            if (_types.TryGetValue(csig, out type))
            {
                return type.EnrollApi(member);
            }
            return false;
        }

        internal bool TryMatchType(string sig, out TypeApi sketch)
        {
            sketch = null;
            return _types.TryGetValue(sig, out sketch);
        }

        public Compatibility IsCompatible(AssemblyApi old)
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();

            if (this.PublicKeyToken != old.PublicKeyToken)
            {
                sb.Append("PublicTokenKey is changed.");
                result = -1;
            }

            foreach (var refe in this.References)
            {
                if (!old._references.Contains(refe))
                {
                    sb.Append(refe.ToString() + " is added.");
                    result = -1;
                }
            }

            foreach (var oldType in old.Types)
            {
                TypeApi newType;
                if (!_types.TryGetValue(oldType.Signature, out newType))
                {
                    sb.Append(oldType.Signature + " can't be found.");
                    result = -1;
                }
                else
                {
                    var comp = newType.IsCompatible(oldType);
                    if (comp.ChangeLevel != ChangeLevel.NoChange)
                        sb.Append(comp.Message);
                    result &= (int)comp.ChangeLevel;
                }
            }

            return new Compatibility((ChangeLevel)result, sb.ToString());
        }

    }
}
