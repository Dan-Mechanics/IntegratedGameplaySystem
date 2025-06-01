using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(EndMenu), fileName = "New " + nameof(EndMenu))]
    public class EndMenu : Menu
    {
        //public Menu menu;
        public GameObject text;

        public override List<object> GetGame()
        {
            List<object> behaviours = base.GetGame();

            IScoreService service = ServiceLocator<IScoreService>.Locate();
            string highscore = service == null ? "no highscore ..." : service.GetHighscore().ToString();
            Utils.MakeText(base.Transform, text, 3f * 30f * Vector2.up).GetComponent<Text>().text = highscore;

            return behaviours;
        }
    }
}