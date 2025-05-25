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
            Place(itemSlot.transform, Vector3.down * 1000f);

            selectionText = Instantiate(textPrefab).GetComponent<TMP_Text>();
            Place(selectionText.transform, Vector3.down * 1000f);

            playerContext.wallet.OnMoneyChanged += WriteText;
            //playerContext.
        }

        private void Place(Transform trans, Vector3 pos)
        {
            trans.SetParent(transform);
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        public void WriteText(int mess, int b) => selectionText.text = mess.ToString();    
        public void WriteItemSlot(PlantBlueprint blueprint) => WriteImage(blueprint.itemSprite);
        public void WriteImage(Sprite sprite) 
        {
            itemSlot.sprite = sprite == null ? emptySprite : sprite;
        }
    }
}