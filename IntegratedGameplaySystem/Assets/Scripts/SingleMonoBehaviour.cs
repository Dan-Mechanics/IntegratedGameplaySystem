using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// SINGLE MONOBEHAVIOUR HERE !! maybe make this the gamemanager or sometihng thatwould make al ittle more sense.
    /// we arent really gaining anything by leaving this as seperate.
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