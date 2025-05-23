using System;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Reference:
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/EventsExample/EventScripts.cs
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// Welcome to C# !!
        /// </summary>
        private static readonly Dictionary<Occasion, Action> events = new Dictionary<Occasion, Action>();

        public static void RaiseEvent(Occasion occasian)
        {
            if (!events.ContainsKey(occasian))
                return;

            events[occasian]?.Invoke();
        }

        public static void AddListener(Occasion occasian, Action listener)
        {
            if (!events.ContainsKey(occasian))
                events.Add(occasian, null);

            events[occasian] += listener;
        }

        public static void RemoveListener(Occasion occasian, Action listener)
        {
            if (!events.ContainsKey(occasian))
                return;

            events[occasian] -= listener;
        }
    }
}