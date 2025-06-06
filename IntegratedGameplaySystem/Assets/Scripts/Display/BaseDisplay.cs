using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class is not perfect but you get the point.
    /// </summary>
    public class BaseDisplay : IDisposable
    {
        public List<IDisposable> Disposables { get; private set; }
        public DisplaySettings Settings { get; private set; }
        public Transform Canvas { get; private set; }

        public BaseDisplay() 
        {
            Disposables = new List<IDisposable>();
            Settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<DisplaySettings>();
            Canvas = Utils.SpawnPrefab(Settings.canvas).transform;
        }

        public void Dispose()
        {
            Disposables.ForEach(x => x.Dispose());
        }

        public static void StringIntoText(string str, Text text) 
        {
            text.text = Utils.IsStringValid(str) ? str : string.Empty;
        }

        public void SettingsStrIntoText(string str, Text text)
        {
            text.text = Utils.IsStringValid(str) ? str : Settings.defaultText;
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

        public void SettingsSpriteIntoImg(Sprite sprite, Image image)
        {
            //image.color = sprite == null ? Color.clear : Color.white;
            image.sprite = sprite == null ? Settings.defaultSprite : sprite;
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

        /*public static Image AddFillImage(Transform canvas, GameObject prefab, Sprite sprite)
        {
            Image image = AddToCanvas<Image>(canvas, prefab);
            image.sprite = sprite;

            image.type = Image.Type.Filled;
            image.fillAmount = 0f;
            image.fillOrigin = 0;
            image.fillMethod = Image.FillMethod.Horizontal;

            return image;
        }*/

        public Image AddFillImage()
        {
            Image image = AddToCanvas<Image>(Canvas, Settings.image);
            image.sprite = Settings.pixel;

            image.type = Image.Type.Filled;
            image.fillAmount = 0f;
            image.fillOrigin = 0;
            image.fillMethod = Image.FillMethod.Horizontal;

            return image;
        }
    }
}