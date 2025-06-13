namespace IntegratedGameplaySystem
{
    public abstract class PlantState : IInteractable, IHoverable, IWaterable, IHarvestable
    {
        protected PlantUnit plant;

        public void PassPlant(PlantUnit plant) 
        {
            this.plant = plant;
        }

        public virtual string GetHoverTitle() => string.Empty;
        public virtual void Harvest() { }
        public virtual void Interact() { }
        public virtual void Tick() { }
        public virtual void Water() { }
    }
}