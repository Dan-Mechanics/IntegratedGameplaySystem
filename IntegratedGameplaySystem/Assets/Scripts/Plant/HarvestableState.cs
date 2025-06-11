namespace IntegratedGameplaySystem
{
    public class HarvestableState : IPlantState
    {
        public PlantCommonality Plant { get; set; }

        public string GetHoverTitle()
        {
            return Plant.flyweight.name;
        }

        public void Harvest() 
        {
            Plant.Progress = 0;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, Plant.flyweight);

            Plant.SetColliderHeight(PlantCommonality.LOWERED_COLLIDER_HEIGHT);

            Plant.SetState(typeof(SoilState));
            Plant.RefreshMaterials();
            Plant.SetWatered(false);
        }

        public void Interact()
        {
            Harvest();
        }

        public void Tick() { }

        public void Water()
        {
            Plant.SetWatered(true);
        }
    }
}