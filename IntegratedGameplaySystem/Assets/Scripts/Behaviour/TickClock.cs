using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class TickClock : IStartable, IFixedUpdatable
    {
        public float interval;
        private readonly Timer timer = new();

        public TickClock(float interval)
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
            EventManager.RaiseEvent(Occasion.TICK);
        }
    }
}