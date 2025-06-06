using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public class BaseDisplay : IDisposable
    {
        public List<IDisposable> Disposables { get; private set; }
        public DisplaySettings BaseSettings { get; private set; }
        public Transform Canvas { get; private set; }

        public BaseDisplay() 
        {
            Disposables = new List<IDisposable>();
            BaseSettings = ServiceLocator<IAssetService>.Locate().GetAssetByType<DisplaySettings>();
            Canvas = Utils.SpawnPrefab(BaseSettings.canvas).transform;
        }

        public void Dispose()
        {
            Disposables.ForEach(x => x.Dispose());
        }

        public static void StringIntoText(string str, Text text) 
        {
            text.text = string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) ? string.Empty : str;
        }

        public static void RangeIntoText(Range range, Text text) => text.text = $"({range.value} / {range.max})";
        public static void FloatIntoText(float value, Text text) => text.text = value.ToString();
        public static void RangeIntoFillImage(Range range, Image fillImg) => fillImg.fillAmount = (float)range.value / range.max;
        public static void IntIntoText(int value, Text text) => text.text = value.ToString();

        public static void SpriteIntoImage(Sprite sprite, Image image) 
        {
            image.color = sprite == null ? Color.clear : Color.white;
            image.sprite = sprite;
        }

        public static void BoolIntoRedText(bool isRed, Text text) => text.color = isRed ? Color.red : Color.black;

        public static void ItemStackIntoSlot(ItemStack stack, Slot slot)
        {
            SpriteIntoImage(stack.item?.Sprite, slot.image);
            IntIntoText(stack.count, slot.text);
            BoolIntoRedText(stack.AtCapacity(), slot.text);
        }

        public static T AddToCanvas<T>(Transform canvas, GameObject prefab)
        {
            Transform transform = Utils.SpawnPrefab(prefab).transform;
            transform.SetParent(canvas);
            transform.localPosition = Vector3.zero;
            return transform.GetComponent<T>();
        }

        public static Image AddFillImage(Transform canvas, GameObject prefab, Sprite sprite)
        {
            Image image = AddToCanvas<Image>(canvas, prefab);
            image.sprite = sprite;

            image.type = Image.Type.Filled;
            image.fillAmount = 0f;
            image.fillOrigin = 0;
            image.fillMethod = Image.FillMethod.Horizontal;

            return image;
        }

        /*public T AddToCanvas<T>(GameObject prefab)
        {
            Transform transform = Utils.SpawnPrefab(prefab).transform;
            transform.SetParent(Canvas);
            transform.localPosition = Vector3.zero;
            return transform.GetComponent<T>();
        }

        public Image AddFillImage(GameObject prefab, Sprite sprite)
        {
            Image image = AddToCanvas<Image>(Canvas, prefab);
            image.sprite = sprite;

            image.type = Image.Type.Filled;
            image.fillAmount = 0f;
            image.fillOrigin = 0;
            image.fillMethod = Image.FillMethod.Horizontal;

            return image;
        }*/
    }
}