using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class Clock : IStartable, IFixedUpdatable, IScoreService
    {
        public Action<float> OnNewScore;
        
        public float interval;
        private readonly Timer timer = new();

        private float score;

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
            score = Time.timeSinceLevelLoad;
            OnNewScore?.Invoke(score);
            
            if (!timer.Tick(Time.fixedDeltaTime))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.TICK);
        }

        public float GetHighscore()
        {
            return score;
        }
    }
}