using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IInputService 
    {
        InputSource GetInputSource(PlayerAction playerAction);
    }
}