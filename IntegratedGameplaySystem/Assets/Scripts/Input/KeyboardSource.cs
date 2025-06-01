using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I know this is a little overcomplicated.
    /// </summary>
    public class KeyboardSource : IPlayerInput
    {
        /// <summary>
        /// Does this deallocate ??
        /// </summary>
        private readonly InputSource forward;
        private readonly InputSource back;
        private readonly InputSource left;
        private readonly InputSource right;

        public KeyboardSource(IInputService inputService)
        {
            //IInputService inputService = ServiceLocator<IInputService>.Locate();

            forward = inputService.GetInputSource(PlayerAction.Forward);
            back = inputService.GetInputSource(PlayerAction.Backward);
            left = inputService.GetInputSource(PlayerAction.Left);
            right = inputService.GetInputSource(PlayerAction.Right);
        }

        public float GetVertical() 
        {
            float z = 0f;

            if (forward.IsPressed)
                z++;

            if (back.IsPressed)
                z--;

            return z;
        }

        public float GetHorizontal()
        {
            float x = 0f;

            if (right.IsPressed)
                x++;

            if (left.IsPressed)
                x--;

            return x;
        }

        public Vector2 GetLookingInput() 
        {
            return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
    }
}