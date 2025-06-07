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

            if (!inactivePool.Contains(output))
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

        private readonly PoolItem[] pool;
        private readonly Dictionary<T, int> dict = new();

        private struct PoolItem 
        {
            public T t;
            public bool active;
        }

        /*public enum Request { Get, Give }
        private struct Command 
        {
            public Request request;
            public T t;
        }*/

        public FastPool(int maxExpected)
        {
            // lol
            if (maxExpected < 1)
                maxExpected = 1;
            
            pool = new PoolItem[maxExpected];
            /*for (int i = 0; i < pool.Length; i++)
            {
                pool[i].t = AllocateNew.Invoke();
                dict.Add(pool[i].t, i);
            }*/
        }

        /// <summary>
        /// Or do it more dynamically
        /// </summary>
        public void Fill() 
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].t = AllocateNew.Invoke();
                dict.Add(pool[i].t, i);
            }
        }

        public void Give(T input)
        {
            pool[dict[input]].t.Disable();
            pool[dict[input]].active = false;
        }

        public void Flush()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].t.Flush();
            }
        }

        public T Get()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (!pool[i].active)
                    return Send(ref pool[i]);
            }

            return Send(ref pool[0]);
        }

        private T Send(ref PoolItem pair) 
        {
            pair.active = true;
            pair.t.Enable();
            return pair.t;
        }
    }
}
