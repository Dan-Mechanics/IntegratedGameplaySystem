namespace IntegratedGameplaySystem
{
    /// <summary>
    /// What's the occasion ?
    /// </summary>
    public enum Occasion
    {
        CloseGame = 1,
        GameOver = 2,
        Tick = 3, // plant tick/
        EarnMoney = 4,
        PickupItem = 5,
        LoseMoney = 6,
        LateTick = 7 // UpgreadeTick
    }
}