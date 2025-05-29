using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Perchance add update here since thats important right.
    /// </summary>
    public interface IInputService : IUpdatable
    {
        InputSource GetInputSource(PlayerAction playerAction);
    }
}