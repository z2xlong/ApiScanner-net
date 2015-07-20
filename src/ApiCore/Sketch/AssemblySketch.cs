using System;
using System.Collections.Generic;
using Microsoft.Cci;
using System.Diagnostics;

namespace ApiCore
{
    public class AssemblySketch : ICompatible<AssemblySketch>, IEquatable<AssemblySketch>
    {
        #region fileds & Properties

        string _str = null;
        readonly HashSet<AssemblySketch> _references = new HashSet<AssemblySketch>();
        readonly Dictionary<string, TypeSketch> _types = new Dictionary<string, TypeSketch>();

        public string Name { get; private set; }

        public Version Version { get; private set; }

        public string Culture { get; private set; }

        public string PublicKeyToken { get; private set; }

        public string Location { get; private set; }

        public IEnumerable<AssemblySketch> References
        {
            get { return _references; }
        }

        public IEnumerable<TypeSketch> Types
        {
            get { return _types.Values; }
        }

        #endregion

        #region ctor

        internal AssemblySketch(IAssembly assembly)
        {
            Debug.Assert(assembly != null, "AssemblySkethc could not initialized from null IAssembly.");

            this.Location = assembly.Location;
            this.Name = assembly.Name.Value;
            this.Version = assembly.Version;
            this.PublicKeyToken = ComputePublicKey(assembly.PublicKeyToken);

            foreach (IAssemblyReference reference in assembly.AssemblyReferences)
                _references.Add(new AssemblySketch(reference.ResolvedAssembly));
        }

        #endregion

        #region overrieded

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(_str))
                _str = string.Format("Name={0},Version={1},Cluture={2},PublicKeyToken={3}",
                this.Name, this.Version, this.Culture, this.PublicKeyToken);

            return _str;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public bool Equals(AssemblySketch other)
        {
            return this.Name == other.Name
                && this.Version == other.Version
                && this.Culture == other.Culture
                && this.PublicKeyToken == other.PublicKeyToken;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AssemblySketch))
                return false;

            return this.Equals((AssemblySketch)obj);
        }

        private string ComputePublicKey(IEnumerable<byte> token)
        {
            byte[] bytes = (new List<byte>(token)).ToArray();
            return BitConverter.ToString(bytes);
        }

        #endregion

        internal void EnrollType(ITypeDefinition typeDefinition)
        {
            TypeSketch type = new TypeSketch(typeDefinition);
            if (!_types.ContainsKey(type.Signature))
            {
                _types[type.Signature] = type;
            }
        }

        internal bool EnrollApi(ITypeDefinitionMember member)
        {
            var csig = ApiHelper.GetSignature(member.ContainingTypeDefinition);
            TypeSketch type;
            if (_types.TryGetValue(csig, out type))
            {
                return type.EnrollApi(member);
            }
            return false;
        }

        internal bool TryMatchType(string sig, out TypeSketch sketch)
        {
            sketch = null;
            return _types.TryGetValue(sig, out sketch);
        }

        public bool IsCompatible(AssemblySketch old, IList<string> incompatibility)
        {
            bool result = true;
            if (this.PublicKeyToken != old.PublicKeyToken)
            {
                incompatibility.Add("PublicTokenKey is changed.");
                result = false;
            }

            foreach (var refe in this.References)
            {
                if (!old._references.Contains(refe))
                {
                    incompatibility.Add(refe.ToString() + " is added.");
                    result = false;
                }
            }

            foreach (var oldType in old.Types)
            {
                TypeSketch newType;
                if (!_types.TryGetValue(oldType.Signature, out newType))
                {
                    incompatibility.Add(oldType.Signature + " can't be found.");
                    result = false;
                }
                else
                    result &= newType.IsCompatible(oldType, incompatibility);
            }

            return result;
        }

    }
}
