using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I wanna use this for input and config loading.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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