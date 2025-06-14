﻿using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SceneSetup), fileName = "New " + nameof(SceneSetup))]
    public class SceneSetup : ScriptableObject
    {
        [Min(10)] public int maxFPS;
        [Min(10f)] public float fixedUpdatesPerSecond;
        public bool mouseLocked;

        public void Setup()
        {
            Debug.Log($"loading {name.ToUpper()}");

            Random.InitState(0);

            Physics.simulationMode = SimulationMode.Script;
            Time.fixedDeltaTime = 1f / fixedUpdatesPerSecond;
            Application.targetFrameRate = maxFPS;

            Cursor.visible = !mouseLocked;
            Cursor.lockState = mouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}