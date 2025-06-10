namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We do interact not primary fire, there is no shooting in thisgame basically.
    /// </summary>
    public enum PlayerAction
    {
        None = 0,
        Interact = 1,
        Forward = 2,
        Backward = 3,
        Left = 4,
        Right = 5,
        Reload = 6,
        Escape = 7,
        Up = 8,
        Down = 9
    }
}