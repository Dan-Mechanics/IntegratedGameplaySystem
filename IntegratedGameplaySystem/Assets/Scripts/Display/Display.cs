﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Reusable class for UI.
    /// </summary>
    public class Display : IDisposable
    {
        public List<IDisposable> Disposables { get; private set; }
        public DisplaySettings Settings { get; private set; }
        public Transform Canvas { get; private set; }

        public Display() 
        {
            Disposables = new List<IDisposable>();
            Settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<DisplaySettings>();
            Canvas = Utils.SpawnPrefab(Settings.canvasPrefab).transform;
        }

        public void Dispose()
        {
            Disposables.ForEach(x => x.Dispose());
        }

        public void SettingsStrIntoText(string str, Text text)
        {
            text.text = Utils.IsStringValid(str) ? str : Settings.defaultText;
        }

        public Image AddFillImage()
        {
            Image image = AddToCanvas<Image>(Canvas, Settings.imagePrefab);
            image.sprite = Settings.pixel;

            image.type = Image.Type.Filled;
            image.fillAmount = 0f;
            image.fillOrigin = 0;
            image.fillMethod = Image.FillMethod.Horizontal;

            return image;
        }

        public static void StringIntoText(string str, Text text) 
        {
            text.text = Utils.IsStringValid(str) ? str : string.Empty;
        }

        public static void RangeIntoText(IntWithMax range, Text text) => text.text = $"({range.value} / {range.max})";
        public static void FloatIntoText(float value, Text text) => text.text = value.ToString();
        public static void RangeIntoFillImage(IntWithMax range, Image fillImg) => fillImg.fillAmount = (float)range.value / range.max;
        public static void IntIntoText(int value, Text text) => text.text = value.ToString();

        public static void SpriteIntoImage(Sprite sprite, Image image) 
        {
            if (sprite == null)
                image.color = Color.clear;

            image.sprite = sprite;
        }

        public static void BoolIntoRedText(bool isRed, Text text) => text.color = isRed ? Color.red : Color.black;

        public static void ItemStackIntoSlot(ItemStack stack, ItemSlot slot)
        {
            SpriteIntoImage(stack.item?.Sprite, slot.image);
            slot.image.color = stack.item == null ? Color.clear : stack.item.ItemTint;

            IntIntoText(stack.count, slot.text);
            BoolIntoRedText(stack.isAtCapacity, slot.text);
        }

        public static T AddToCanvas<T>(Transform canvas, GameObject prefab)
        {
            Transform transform = Utils.SpawnPrefab(prefab).transform;
            transform.SetParent(canvas);
            transform.localPosition = Vector3.zero;
            return transform.GetComponent<T>();
        }
    }
}