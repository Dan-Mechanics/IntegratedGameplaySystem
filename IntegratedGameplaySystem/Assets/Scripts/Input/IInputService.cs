namespace IntegratedGameplaySystem
{
    public interface IInputService : IUpdatable, IDisposable
    {
        InputSource GetInputSource(PlayerAction playerAction);
    }
}