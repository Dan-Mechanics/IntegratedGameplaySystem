namespace IntegratedGameplaySystem
{
    public class TimeHandler 
    {
        private readonly Timer timer;
        private readonly float interval;

        public TimeHandler(float interval)
        {
            this.interval = interval;
            timer = new Timer();
            timer.SetValue(interval);
        }

        public void Update(float dt) 
        {
            if (timer.Tick(dt))
                return;

            timer.SetValue(interval);
            EventManager.RaiseEvent(Occasion.TICK);
        }
    }
}