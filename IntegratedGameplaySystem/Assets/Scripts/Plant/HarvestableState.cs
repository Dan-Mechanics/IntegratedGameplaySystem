namespace IntegratedGameplaySystem
{
    public class HarvestableState : PlantState
    {
        public override string GetHoverTitle() => plant.flyweight.name;

        public override void Harvest() 
        {
            plant.Progress = 0;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, plant.flyweight);

            plant.SetColliderHeight(PlantUnit.LOWERED_COLLIDER_HEIGHT);

            plant.SetState(typeof(SoilState));
            plant.RefreshMaterials();
            plant.SetWatered(false);
        }

        public override void Interact() => Harvest();
        public override void Water() => plant.SetWatered(true);
    }
}