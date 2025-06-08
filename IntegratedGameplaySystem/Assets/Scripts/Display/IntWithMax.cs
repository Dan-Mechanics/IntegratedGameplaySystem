using UnityEngine;

namespace IntegratedGameplaySystem
{
    public struct IntWithMax
    {
        public int value;
        public int max;

        public void Set(int value, int max)
        {
            this.value = value;
            this.max = max;
        }

        public void Clamp() 
        {
            value = Mathf.Clamp(value, 0, max);
        }
    }
}