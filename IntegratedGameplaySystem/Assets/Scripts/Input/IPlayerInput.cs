using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I wanted to leave the door open for controller 
    /// input devices potentially.
    /// </summary>
    public interface IPlayerInput
    {
        float GetVertical();
        float GetHorizontal();
        Vector2 GetLookingInput();
    }
}