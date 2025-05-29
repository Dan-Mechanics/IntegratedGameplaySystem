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
            
            string txt = ServiceLocator<IAssetService>.Locate().GetByType<TextAsset>().text;
            string[] lines = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] tokens = line.Split(' ');
                bindings.Add(new Binding(tokens[0], Utils.StringToEnum<PlayerAction>(tokens[1])));
            }

            return bindings;
        }
    }
}