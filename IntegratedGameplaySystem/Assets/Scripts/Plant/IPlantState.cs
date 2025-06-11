namespace IntegratedGameplaySystem
{
    public interface IPlantState : IInteractable, IHoverable, IWaterable, IHarvestable
    {
        PlantUnit Plant { get; set; }
        void Tick();
    }
}