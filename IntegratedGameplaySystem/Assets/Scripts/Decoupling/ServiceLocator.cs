using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I wanna use this for input and config loading.
    /// CREDIT: HKU CLASSES.
    /// 
    /// Perhaps i should add more stuff to the folder this is in including interfaces but ok.
    /// maybe then it makes it sligjhtly more clear which things use this right. yeah.
    /// but thats for later now i just wanna make the plants work in general.
    /// 
    /// 
    /// Note to self: we can use servicelocators as tools for persistant memory.
    /// </summary>
    public static class ServiceLocator<T>
    {
        private static T instance;

        /// <summary>
        /// Please cache. Or dont.
        /// </summary>
        public static T Locate()
        {
            if (instance == null)
                throw new Exception("No instance provided yet !!");

            return instance;
        }

        public static bool HasBeenProvided() => instance != null;

        public static void Provide(T service) 
        {
            instance = service;
        }
    }
}