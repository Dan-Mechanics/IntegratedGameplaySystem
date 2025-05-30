using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IGame 
    {
        List<object> GetGameBehaviours();
    }

    public class FarmingFrenzy : IGame 
    {
        /// <summary>
        /// Cosndier moving this somewhere else.
        /// Litterally lvoe coding silly things like this:
        /// And suddenly im me again.
        /// 
        /// MASSIVE COUPLING EMERGY HERE !!
        /// </summary>
        public List<object> GetGameBehaviours()
        {
            List<object> behaviours = new()
            {
                ServiceLocator<IInputService>.Locate(),
                new PlayerMovement(new KeyboardSource()),
                new TestingFeatures(),
                new TickClock()
            };

            var interactor = new Interactor();
            behaviours.Add(interactor);

            var wallet = new Wallet();
            behaviours.Add(wallet);

            var plantSpecies = ServiceLocator<IAssetService>.Locate().GetCollectionType<PlantSpeciesProfile>();
            plantSpecies.ForEach(x => x.Populate(behaviours));

            var display = new Display(interactor, wallet);
            behaviours.Add(display);

            return behaviours;
        }
    }
}