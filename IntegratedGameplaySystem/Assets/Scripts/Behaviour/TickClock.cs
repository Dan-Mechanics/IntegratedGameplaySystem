using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class TickClock : IStartable, IFixedUpdatable, IHighscoreService
    {
        public Action<float> OnNewTime;
        
        public float interval;
        private readonly Timer timer = new();

        private float time;

        public TickClock(float interval = 1f)
        {
            this.interval = interval;   
        }

        public void Start()
        {
            timer.SetValue(interval);
        }

        public void FixedUpdate() 
        {
            time = Time.time;
            OnNewTime?.Invoke(time);
            
            if (!timer.Tick(Time.fixedDeltaTime))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.TICK);
        }

        public float GetHighscore()
        {
            return time;
        }
    }
}