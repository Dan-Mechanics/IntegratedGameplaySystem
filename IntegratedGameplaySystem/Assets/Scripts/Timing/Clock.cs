using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I feel like this *could* be seperated into two different classes.
    /// </summary>
    public class Clock : IStartable, IFixedUpdatable
    {
        private readonly Timer tickTimer = new Timer();
        private readonly float interval;

        public Clock(float interval)
        {
            this.interval = interval;   
        }

        public void Start()
        {
            tickTimer.SetValue(interval);
        }

        public void FixedUpdate() 
        {
            if (!tickTimer.Tick(Time.fixedDeltaTime))
                return;

            EventManager.RaiseEvent(Occasion.Tick);
            EventManager.RaiseEvent(Occasion.LateTick);
            tickTimer.SetValue(interval);
        }
    }
}