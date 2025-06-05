using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I feel like this *could* be seperated into two different classes.
    /// </summary>
    public class Clock : IStartable, IFixedUpdatable, IScoreService, IChangeTracker<float>
    {
        public Action<float> OnNewTime;
         
        public float interval;
        private readonly Timer timer = new();

        private float time;

        public event Action<float> OnChange;

        public Clock(float interval)
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
            OnChange?.Invoke(time);
            
            if (!timer.Tick(Time.fixedDeltaTime))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.Tick);
        }

        public float GetScore()
        {
            return time;
        }
    }
}