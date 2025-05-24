using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Also useful for creatures.
    /// </summary>
    public class FixedTicks
    {
        public float Timer => timer;
        
        private readonly float interval = 0.02f;
        private float timer;
        private int counter;

        public FixedTicks(float interval)
        {
            this.interval = interval;
        }

        /// <summary>
        /// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.Simulate.html
        /// </summary>
        public int GetTicksCount(float dt)
        {
            counter = 0;
            timer += dt;

            while (timer >= interval)
            {
                timer -= interval;

                counter++;
            }

            return counter;
        }
    }
}