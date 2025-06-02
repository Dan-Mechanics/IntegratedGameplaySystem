using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(WalletSettings), fileName = "New " + nameof(WalletSettings))]
    public class WalletSettings : ScriptableObject
    {
        public GameObject sellPointPrefab;
        public int moneyToWin;
    }
}