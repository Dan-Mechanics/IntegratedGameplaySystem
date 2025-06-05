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
            string scoreText = service == null ? "no score yet ..." : service.GetScore().ToString();

            Text text = Utils.AddToCanvas<Text>(canvas, textPrefab);
            text.text = scoreText;

            EasyUI ui = new EasyUI(text.rectTransform);
            ui.SnapTo(Snap.Center, 90f * Vector2.up);
        }
    }
}