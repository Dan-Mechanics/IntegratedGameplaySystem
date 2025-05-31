using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This is not lisov substiturion.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(EndMenu), fileName = "New " + nameof(EndMenu))]
    public class EndMenu : Menu
    {
        public GameObject text;

        public override List<object> GetGame()
        {
            List<object> behaviours = base.GetGame();

            IHighscoreService service = ServiceLocator<IHighscoreService>.Locate();
            string highscore = service == null ? "no highscore ..." : service.GetHighscore().ToString();
            Utils.MakeText(canvasTrans, text, 3f * 30f * Vector2.up).GetComponent<Text>().text = highscore;

            return behaviours;
        }
    }
}