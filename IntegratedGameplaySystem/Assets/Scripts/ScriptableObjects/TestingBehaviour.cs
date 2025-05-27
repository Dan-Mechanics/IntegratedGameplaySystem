using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(TestingBehaviour), fileName = "New " + nameof(TestingBehaviour))]
    public class TestingBehaviour : BaseBehaviour
    {
        public override void Start()
        {
            base.Start();

            ServiceLocator<IInputService>.Locate().GetInputSource(PlayerAction.Reload).OnDown += Reload;
            ServiceLocator<IInputService>.Locate().GetInputSource(PlayerAction.Escape).OnDown += Quit;
        }

        private void Reload() 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Quit() 
        {
            Application.Quit();
        }

        public override void Disable()
        {
            base.Disable();

            ServiceLocator<IInputService>.Locate().GetInputSource(PlayerAction.Reload).OnDown -= Reload;
            ServiceLocator<IInputService>.Locate().GetInputSource(PlayerAction.Escape).OnDown -= Quit;
        }
    }
}