using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Score : IFixedUpdatable, IScoreService, IChangeTracker<float>
    {
        public event Action<float> OnChange;
        private float time;

        public void FixedUpdate() 
        {
            time = Time.timeSinceLevelLoad;
            OnChange?.Invoke(time);
        }

        public float GetScore() => time;
    }
}