using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IItemArchitype
    {
        Sprite Sprite { get; }
        Color ItemTint { get; }
        int MonetaryValue { get; }
    }
}
