using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IInputService 
    {
        InputState GetAction(PlayerAction playerAction);
    }
}