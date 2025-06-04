using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I know this is a little overcomplicated.
    /// </summary>
    public interface IPlayerInput
    {
        float GetVertical();
        float GetHorizontal();
        Vector2 GetLookingInput();
    }
}