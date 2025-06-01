using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Menu), fileName = "New " + nameof(Menu))]
    public class Menu : Scene
    {
        public Transform Transform { get; private set; }
        [SerializeField] private GameObject canvas = default;
        private Button button;
        
        public override List<object> GetGame()
        {
            List<object> behaviours = base.GetGame();

            Transform = Utils.SpawnPrefab(canvas).transform;
            button = Transform.GetComponentInChildren<Button>();
            button.onClick.AddListener(NextScene);

            return behaviours;
        }
        public override void Dispose()
        {
            base.Dispose();
            button.onClick.RemoveListener(NextScene);
        }

    }
}