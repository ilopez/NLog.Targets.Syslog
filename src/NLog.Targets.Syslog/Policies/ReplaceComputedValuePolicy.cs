// Licensed under the BSD license
// See the LICENSE file in the project root for more information

using NLog.Common;
using System.Text.RegularExpressions;
using NLog.Targets.Syslog.Settings;

namespace NLog.Targets.Syslog.Policies
{
    internal class ReplaceComputedValuePolicy
    {
        private readonly EnforcementConfig enforcementConfig;
        private readonly string replaceWith;

        public ReplaceComputedValuePolicy(EnforcementConfig enforcementConfig, string replaceWith)
        {
            this.enforcementConfig = enforcementConfig;
            this.replaceWith = replaceWith;
        }

        public bool IsApplicable()
        {
            return enforcementConfig.ReplaceInvalidCharacters;
        }

        public string Apply(string s, string searchFor)
        {
            if (string.IsNullOrEmpty(searchFor) || string.IsNullOrEmpty(replaceWith) || s.Length == 0)
                return s;

            var replaced = Regex.Replace(s, searchFor, replaceWith);
            InternalLogger.Trace("[Syslog] Replaced '{0}' (if found) with '{1}' given computed value '{2}': '{3}'", searchFor, replaceWith, s, replaced);
            return replaced;
        }
    }
}