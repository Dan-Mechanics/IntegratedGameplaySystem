namespace IntegratedGameplaySystem
{
    public interface IPoolable
    {
        void Disable();
        void Enable();
        void Flush();
    }
}
