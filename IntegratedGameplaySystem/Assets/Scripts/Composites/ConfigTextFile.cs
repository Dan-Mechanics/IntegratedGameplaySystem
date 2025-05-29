using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class ConfigTextFile : IBindingsSource
    {
        public List<Binding> GetBindings()
        {
            string txt = ServiceLocator<IAssetService>.Locate().GetByType<TextAsset>().text;
            // loop

            // loop

            // parse.

            return null;
        }
    }
}