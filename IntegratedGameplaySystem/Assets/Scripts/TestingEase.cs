using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(TestingEase), fileName = "New " + nameof(TestingEase))]
    public class TestingEase : BaseBehaviour
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