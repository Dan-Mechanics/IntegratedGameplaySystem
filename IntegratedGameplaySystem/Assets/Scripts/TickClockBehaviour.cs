using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(TickClockBehaviour), fileName = "New " + nameof(TickClockBehaviour))]
    public class TickClockBehaviour : BaseBehaviour
    {
        public float interval;
        private readonly Timer timer = new Timer();

        public override void Start()
        {
            base.Start();
            timer.SetValue(interval);
        }

        public override void FixedUpdate() 
        {
            if (!timer.Tick(Time.fixedDeltaTime))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.TICK);
        }
    }
}