using System;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IPoolService<T> where T : IPoolable 
    {
        event Func<T> AllocateNew;
        void Give(T input);
        T Get();
        void Flush();
    }
    
    public class ObjectPool<T> : IPoolService<T> where T : IPoolable
    {
        public event Func<T> AllocateNew;

        private readonly List<T> inactivePool = new();
        private readonly List<T> activePool = new();

        public void Give(T input)
        {
            input.Disable();
            activePool.Remove(input);

            if (!inactivePool.Contains(input))
                inactivePool.Add(input);
        }

        public void Flush()
        {
            inactivePool.ForEach(x => x.Flush());
            activePool.ForEach(x => x.Flush());

            inactivePool.Clear();
            activePool.Clear();
        }

        public T Get()
        {
            T output;

            if (inactivePool.Count <= 0)
            {
                output = AllocateNew.Invoke();
            }
            else 
            {
                output = inactivePool[0];
                inactivePool.RemoveAt(0);
            }

            output.Enable();

            if (!activePool.Contains(output))
                activePool.Add(output);

            return output;
        }
    }

    /// <summary>
    /// Changes: array, preallocate, spread burden, event queue !!
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

        /// <summary>
        /// NOTE: int.maxvalue for index ??
        /// </summary>
        public T Get()
        {
            T t = pool[index % pool.Length];
            t.Enable();
            index++;

            return t;
        }
    }
}
