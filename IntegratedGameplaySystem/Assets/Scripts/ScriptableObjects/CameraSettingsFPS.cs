using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(CameraSettingsFPS), fileName = "New " + nameof(CameraSettingsFPS))]
    public class CameraSettingsFPS : ScriptableObject
    {
        public float sens;
        public float eyesHeight;
    }
}