using System;

namespace IntegratedGameplaySystem
{
    public interface IPoolService<T> where T : IPoolable 
    {
        event Func<T> AllocateNew;
        void Give(T input);
        T Get();
        void Flush();
    }
}
