using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IPlayerInput
    {
        float GetVertical();
        float GetHorizontal();
        Vector2 GetLookingInput();
    }
}