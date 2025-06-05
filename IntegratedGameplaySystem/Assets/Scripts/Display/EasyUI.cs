using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public enum Snap { Center, Left, Right, Bottom, Top }
    
    public class EasyUI
    {
        private RectTransform rect;

        public EasyUI(RectTransform rect)
        {
            Set(rect);
        }

        public void Set(RectTransform rect) => this.rect = rect;

        public float GetHeight()
        {
            return rect.sizeDelta.y;
        }

        public float GetWidth()
        {
            return rect.sizeDelta.x;
        }

        public void SnapTo(Snap snap) 
        {
            switch (snap)
            {
                case Snap.Center:
                    SnapToCenter();
                    break;
                case Snap.Left:
                    SnapToLeft();
                    break;
                case Snap.Right:
                    SnapToRight();
                    break;
                case Snap.Bottom:
                    SnapToBottom();
                    break;
                case Snap.Top:
                    SnapToTop();
                    break;
                default:
                    break;
            }
        }

        public void SnapTo(Snap snap, Vector2 offset)
        {
            SnapTo(snap);
            SetOffset(offset);
        }

        public void SnapToLeft()
        {
            rect.pivot = new Vector2(0f, 0.5f);
            UpdateAnchor();
        }

        public void SnapToRight()
        {
            rect.pivot = new Vector2(1f, 0.5f);
            UpdateAnchor();
        }

        public void SnapToBottom()
        {
            rect.pivot = new Vector2(0.5f, 0f);
            UpdateAnchor();
        }

        public void SnapToTop()
        {
            rect.pivot = new Vector2(0.5f, 1f);
            UpdateAnchor();
        }

        public void SnapToCenter()
        {
            rect.pivot = new Vector2(0.5f, 0.5f);
            UpdateAnchor();
        }

        public void SetOffset(Vector2 offset) 
        {
            rect.anchoredPosition = offset;
        }

        private void UpdateAnchor()
        {
            rect.anchorMin = rect.pivot;
            rect.anchorMax = rect.pivot;
            SetOffset(Vector2.zero);
        }
    }
}