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
            string scoreText = service == null ? 
                "no score yet ..." :
                $"your time: {service.GetScore()}";

            Text text = BaseDisplay.AddToCanvas<Text>(canvas, textPrefab);
            text.text = scoreText;

            EasyRect ui = new EasyRect(text.rectTransform);
            ui.SnapTo(Snap.Center, 90f * Vector2.up);
        }
    }
}