namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We want to leave the door open for inventories that hold more
    /// than one ItemStack.
    /// </summary>
    public interface IItemHolder
    {
        ItemStack[] GetItems();
        void Clear();
    }
}