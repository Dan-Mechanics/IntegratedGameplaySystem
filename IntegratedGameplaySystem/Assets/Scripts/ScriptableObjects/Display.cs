using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I want this class to pipe info to all UI shit.
    /// This class might need a reference to the canvas? or like 
    /// it makes shit in the canvas itslef but makign UI code is kinda annoying
    /// so i would rahter have us not do that right.
    /// Maybe if we have some clever solution for assets??
    /// Where like we just spawn it in and it works??
    /// </summary>
    public class Display
    {
        public GameObject itemSlotPrefab;
        public GameObject textPrefab;
        public Sprite emptySprite;
        //public PlayerContext playerContext;
        
        private void Place(Transform trans, Vector3 pos)
        {
            //trans.SetParent(transform);
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}