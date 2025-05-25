using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(DisplayBehaviour), fileName = "New " + nameof(DisplayBehaviour))]
    public class DisplayBehaviour : BaseBehaviour
    {
        public GameObject itemSlotPrefab;
        public GameObject textPrefab;
        public Sprite emptySprite;
        public PlayerContext playerContext;
        // basically we create some bullshit.

        private Image itemSlot;  
        private TMP_Text selectionText;

        public override void Start()
        {
            base.Start();

            // create some shit here.

            itemSlot = Instantiate(itemSlotPrefab).GetComponent<Image>();
            Place(itemSlot.transform);

            selectionText = Instantiate(textPrefab).GetComponent<TMP_Text>();
            Place(selectionText.transform);

            //playerContext.
        }

        private void Place(Transform some)
        {
            some.SetParent(transform);
            some.localPosition = Vector3.zero;
            some.localRotation = Quaternion.identity;
            some.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }

        public void WriteText(string mess) => selectionText.text = mess;    
        public void WriteItemSlot(PlotBehaviour plotBehaviour) => WriteImage(plotBehaviour.itemSprite);
        public void WriteImage(Sprite sprite) 
        {
            itemSlot.sprite = sprite == null ? emptySprite : sprite;
        }
    }
}