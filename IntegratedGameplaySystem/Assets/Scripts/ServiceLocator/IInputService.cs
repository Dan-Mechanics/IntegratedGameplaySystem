using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IInputService 
    {
        InputEvents GetAction(PlayerAction playerAction);
    }
}