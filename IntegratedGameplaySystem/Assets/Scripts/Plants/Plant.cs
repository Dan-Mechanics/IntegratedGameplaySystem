using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(Plant), fileName = "New " + nameof(Plant))]
    public class Plant : BaseBehaviour
    {
        public int size;
        public int index;

        // Place ...
    }
}