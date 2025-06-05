using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public struct Range
    {
        public int value;
        public int max;

        public void Set(int min, int max)
        {
            this.value = min;
            this.max = max;
        }

        public void Clamp() 
        {
            value = Mathf.Clamp(value, 0, max);
        }
    }
}