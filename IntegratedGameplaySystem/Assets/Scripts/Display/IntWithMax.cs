using UnityEngine;

namespace IntegratedGameplaySystem
{
    public struct IntWithMax
    {
        public int min;
        public int max;

        public void Set(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public void Clamp() 
        {
            min = Mathf.Clamp(min, 0, max);
        }
    }
}