using UnityEngine;
using UnityEngine.SceneManagement;

namespace Equilibrium
{
    public class MacroGameHandler : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 300;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                Reload();

            if (Input.GetKeyDown(KeyCode.Escape))
                Quit();
        }

        public void Reload() => Switch(SceneManager.GetActiveScene().name);

        public void Quit() 
        {
            print("quitting game ...");
            Application.Quit();
        }

        public void Switch(string name)
        {
            if (name == "Quit")
            {
                Quit();
                return;
            }
            
            SceneManager.LoadScene(name);
        }
    }
}