using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// SINGLE MONOBEHAVIOUR HERE !!
    /// </summary>
    public class SingleMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private GameContext gameContext = default;

        private void Start() => gameContext.Start();
        private void Update() => gameContext.Update();
        private void OnDisable() => gameContext.Dispose();
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);
    }
}