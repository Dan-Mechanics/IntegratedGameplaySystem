using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Object Pool with ringbuffer.
    /// </summary>
    public class FastPool<T> : IPoolService<T> where T : IPoolable
    {
        public event Func<T> AllocateNew;
        private readonly T[] pool;
        private int index;

        public FastPool(int size)
        {
            if (size < 1)
                size = 1;
            
            pool = new T[size];
        }

        public void Populate() 
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = AllocateNew.Invoke();
            }
        }

        public void Give(T t)
        {
            t.Disable();
        }

        public void Flush()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].Flush();
            }
        }

        public T Get()
        {
            T t = pool[index % pool.Length];
            t.Enable();
            index++;

            return t;
        }
    }
}
