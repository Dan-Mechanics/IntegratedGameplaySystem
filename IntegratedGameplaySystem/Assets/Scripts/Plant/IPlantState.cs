namespace IntegratedGameplaySystem
{
    public interface IPlantState : IInteractable, IHoverable, IWaterable, IHarvestable
    {
        PlantCommonality Plant { get; set; }
        void Tick();
    }
}