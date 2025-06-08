using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class ConfigTextFile : IBindingsSource
    {
        /// <summary>
        /// https://stackoverflow.com/questions/1547476/split-a-string-on-newlines-in-net
        /// </summary>
        public List<Binding> GetBindings()
        {
            List<Binding> bindings = new List<Binding>();

            string txt = ServiceLocator<IAssetService>.Locate().GetAssetByType<TextAsset>().text;
            string[] lines = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (!Utils.IsStringValid(line))
                    continue;

                if (line == "done")
                    break;

                // Ignore statment
                if (line.Contains('!'))
                    continue;

                // Comments
                if (line.Length > 0 && line[0] == '#')
                    continue;

                string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length < 2)
                    continue;
                
                Debug.Log(line);

                // In the future you could say take 2 character only and then define those in the split.
                bindings.Add(new Binding(tokens[0], Utils.StringToEnum<PlayerAction>(tokens[1])));
            }

            return bindings;
        }
    }
}