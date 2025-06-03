using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class HoldingHandler : MonoBehaviour
    {
        // EITHER Evnet onholdingChange or some typa interface so other scripts can get a reference
        // to this mainly the interactor and the display
        
        public PlantBlueprint holding;
        public int count;
        // and then the max count in in the thing.
    }
}