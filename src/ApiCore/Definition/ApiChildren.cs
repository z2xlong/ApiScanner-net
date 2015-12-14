using System;
using System.Collections.Generic;
using System.Text;

namespace ApiScanner.Core
{
    public class ApiChildren : ICompatible<ApiChildren>
    {
        Dictionary<string, TypeMemberApi> _apis = new Dictionary<string, TypeMemberApi>();

        public bool NewMemberIsCompatible { get; private set; }

        public ApiChildren() : this(true) { }

        public ApiChildren(bool newMemberIsCompatible)
        {
            this.NewMemberIsCompatible = NewMemberIsCompatible;
        }

        public bool Enroll(TypeMemberApi api)
        {
            if (_apis.ContainsKey(api.Signature))
                return false;

            _apis[api.Signature] = api;
            return true;
        }

        public Compatibility IsCompatible(ApiChildren old)
        {
            var sigs = GetApiSignatures();
            ChangeLevel level = ChangeLevel.NoChange;
            StringBuilder sb = new StringBuilder();

            foreach (var oldApi in old._apis.Values)
            {
                if (!sigs.Contains(oldApi.Signature))
                {
                    sb.Append(oldApi.Signature + " is removed.");
                    level = ChangeLevel.Broken;
                }
                else
                {
                    sigs.Remove(oldApi.Signature);
                    var comp = _apis[oldApi.Signature].IsCompatible(oldApi);
                    if (comp.ChangeLevel != 0)
                    {
                        sb.Append(comp.Message);
                        level = comp.ChangeLevel;
                    }
                }
            }

            if (!this.NewMemberIsCompatible && sigs.Count > 0)
            {
                level = ChangeLevel.Broken;
                foreach (string sig in sigs)
                    sb.Append(string.Format("\n{0} is added.", sig));
            }

            return new Compatibility(level, sb.ToString());
        }

        private HashSet<string> GetApiSignatures()
        {
            HashSet<string> sigs = new HashSet<string>();
            foreach (var api in _apis.Keys)
                sigs.Add(api);

            return sigs;
        }
    }
}
