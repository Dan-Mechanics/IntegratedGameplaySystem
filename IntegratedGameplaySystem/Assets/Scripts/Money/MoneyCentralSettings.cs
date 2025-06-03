using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(MoneyCentralSettings), fileName = "New " + nameof(MoneyCentralSettings))]
    public class MoneyCentralSettings : ScriptableObject
    {
        public GameObject prefab;
        public int moneyToWin;
    }
}