using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I feel like this *could* be seperated into two different classes.
    /// </summary>
    public class Clock : IStartable, IFixedUpdatable
    {
        public float interval;
        private readonly Timer timer = new();

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
            if (!timer.Tick(Time.fixedDeltaTime))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.Tick);
            EventManager.RaiseEvent(Occasion.LateTick);
        }
    }
}