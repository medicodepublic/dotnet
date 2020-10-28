using System.Collections.Generic;

namespace Structurizr.Core.Util
{
    public class DictionaryUtils
    {
        public static Dictionary<string, string> Create(params string[] nameValuePairs)
        {
            var map = new Dictionary<string, string>();

            if (nameValuePairs != null)
                foreach (var nameValuePair in nameValuePairs)
                {
                    var tokens = nameValuePair.Split('=');
                    if (tokens.Length == 2) map[tokens[0]] = tokens[1];
                }

            return map;
        }
    }
}