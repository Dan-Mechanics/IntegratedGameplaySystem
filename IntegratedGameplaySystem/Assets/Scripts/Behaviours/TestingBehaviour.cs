using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(TestingBehaviour), fileName = "New " + nameof(TestingBehaviour))]
    public class TestingBehaviour : BaseBehaviour
    {
        public override void Update()
        {
            base.Update();
            
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}