namespace IntegratedGameplaySystem
{
    public class Timer
    {
        public float Value => value;
        
        private float value;
        private bool hasBeenSet;
        
        public bool Tick(float interval) 
        {
            if (!hasBeenSet)
                return false;
            
            value -= interval;
            return value <= 0f;
        }

        public virtual void SetValue(float value) 
        {
            this.value = value;
            hasBeenSet = true;
        }

        public void DisableUntilSet() 
        {
            hasBeenSet = false;
        }
    }
}