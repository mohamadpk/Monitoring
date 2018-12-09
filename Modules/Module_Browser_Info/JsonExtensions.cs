using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info
{
    /// <summary>
    /// for search node in json file
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Finds the tokens.
        /// </summary>
        /// <param name="containerToken">The container token.</param>
        /// <param name="name">The name.</param>
        /// <returns>return list of token finded</returns>
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);
            return matches;
        }

        /// <summary>
        /// Finds the tokens.
        /// </summary>
        /// <param name="containerToken">The container token.</param>
        /// <param name="name">The name.</param>
        /// <param name="matches">The matches.</param>
        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }
    }
}
