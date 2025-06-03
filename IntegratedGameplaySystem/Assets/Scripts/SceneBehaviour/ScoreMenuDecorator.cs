using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(ScoreMenuDecorator), fileName = "New " + nameof(ScoreMenuDecorator))]
    public class ScoreMenuDecorator : ScriptableObject, IMenuDecorator
    {
        public GameObject textPrefab;

        public void Decorate(List<object> components, Transform canvas)
        {
            IScoreService service = ServiceLocator<IScoreService>.Locate();
            string score = service == null ? "no highscore ..." : service.GetScore().ToString();
            Utils.AddTextToCanvas(canvas, textPrefab, 3f * 30f * Vector2.up).GetComponent<Text>().text = score;
        }
    }
}