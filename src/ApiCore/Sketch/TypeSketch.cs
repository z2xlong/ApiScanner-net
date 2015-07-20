using System;
using System.Collections.Generic;
using Microsoft.Cci;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCore
{
    public class TypeSketch : ICompatible<TypeSketch>
    {
        Dictionary<string, Api> _apis = new Dictionary<string, Api>();
        HashSet<string> _parents = new HashSet<string>();
        Dictionary<string, string> _genericConstraints = new Dictionary<string, string>();

        public string Signature { get; private set; }

        public IEnumerable<string> Parents
        {
            get { return _parents; }
        }

        public IEnumerable<Api> Apis
        {
            get { return _apis.Values; }
        }

        public IEnumerable<KeyValuePair<string, string>> GenericConstraints
        {
            get { return _genericConstraints; }
        }

        internal TypeSketch(ITypeDefinition definition)
        {
            this.Signature = ApiHelper.GetSignature(definition);

            foreach (var parent in definition.BaseClasses)
                _parents.Add(ApiHelper.GetSignature(parent.ResolvedType));

            foreach (var genpar in definition.GenericParameters)
                _genericConstraints.Add(genpar.Name.Value, ApiHelper.PrintConstraints(genpar));
        }

        internal bool EnrollApi(ITypeDefinitionMember member)
        {
            Api api = new Api(member);
            if (_apis.ContainsKey(api.Signature))
                return false;

            _apis[api.Signature] = api;
            return true;
        }

        public bool IsCompatible(TypeSketch old, IList<string> incompatibility)
        {
            bool result = true;
            if (this.Signature != old.Signature)
            {
                incompatibility.Add(this.Signature);
                result = false;
            }

            foreach (var parent in old.Parents)
            {
                if (!_parents.Contains(parent))
                {
                    incompatibility.Add(this.Signature + ", base classes is changed.");
                    result = false;
                }
            }

            foreach (var genpar in old._genericConstraints)
            {
                if (_genericConstraints[genpar.Key] != genpar.Value)
                {
                    incompatibility.Add(string.Format("{0} {1} where : {2} is changed.", this.Signature, genpar.Key, genpar.Value));
                    result = false;
                }
            }

            foreach (var oldApi in old.Apis)
            {
                Api newApi;
                if (!_apis.TryGetValue(oldApi.Signature, out newApi))
                {
                    incompatibility.Add(oldApi.Signature + " is removed.");
                    result = false;
                }
                else
                    result &= newApi.IsCompatible(oldApi, incompatibility);
            }

            return result;
        }
    }
}
