using UnityEngine;

namespace IntegratedGameplaySystem
{
    public static class ServiceLocator<T>
    {
        public static T instance;
        public static T GetService
        {
            get
            {
                if (instance == null)
                    Debug.LogError("No instance provided yet !!");

                return instance;
            }
        }

        public static void Provide(T service) 
        {
            instance = service;
        }
    }
}