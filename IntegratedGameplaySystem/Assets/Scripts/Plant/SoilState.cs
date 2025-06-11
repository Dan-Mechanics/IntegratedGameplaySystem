namespace IntegratedGameplaySystem
{
    public class SoilState : IPlantState
    {
        public PlantUnit Plant { get; set; }

        public string GetHoverTitle()
        {
            return $"barren {Plant.flyweight.name} soil";
        }

        public void Harvest() { }

        public void Tick() { }

        public void Interact()
        {
            Plant.SetState(typeof(GrowingState));
            Plant.RefreshMaterials();
        }

        public void Water() 
        {
            Plant.SetWatered(true);
        }
    }
}