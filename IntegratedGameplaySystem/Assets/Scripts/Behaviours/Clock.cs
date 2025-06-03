using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class Clock : IStartable, IFixedUpdatable, IScoreService
    {
        public Action<float> OnNewTime;
        
        public float interval;
        private readonly Timer timer = new();

        private float time;

        public Clock(float interval = 1f)
        {
            this.interval = interval;   
        }

        public void Start()
        {
            timer.SetValue(interval);
        }

        public void FixedUpdate() 
        {
            time = Time.timeSinceLevelLoad;
            OnNewTime?.Invoke(time);
            
            if (!timer.Tick(Time.fixedDeltaTime))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.TICK);
        }

        public float GetScore()
        {
            return time;
        }
    }
}