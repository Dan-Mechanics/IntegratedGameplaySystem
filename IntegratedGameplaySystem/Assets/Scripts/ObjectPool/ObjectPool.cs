﻿using System;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
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
}
