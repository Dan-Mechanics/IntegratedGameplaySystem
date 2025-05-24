using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// In the future this should be with InputHandler but ok.
    /// Make it more general to give the string names.
    /// It would be cool if i had aaaaaaaaaaaaaaaaa config system POGGG
    /// that might also be fun #noburnout divas.
    /// 
    /// TEMP FILE !!
    /// </summary>
    public class PlayerInput
    {
        private readonly InputHandler inputHandler;

        private bool forward;
        private bool back;
        private bool left;
        private bool right;

        public PlayerInput(InputHandler inputHandler)
        {
            this.inputHandler = inputHandler;

            inputHandler.GetInputEvents(PlayerAction.Forward).OnChange += OnForward;
            inputHandler.GetInputEvents(PlayerAction.Backward).OnChange += OnBack;
            inputHandler.GetInputEvents(PlayerAction.Left).OnChange += OnLeft;
            inputHandler.GetInputEvents(PlayerAction.Right).OnChange += OnRight;
        }

        private void OnForward(bool value) => forward = value;
        private void OnBack(bool value) => back = value;
        private void OnLeft(bool value) => left = value;
        private void OnRight(bool value) => right = value;

        public float Vertical() 
        {
            float z = 0f;

            if (forward)
                z++;

            if (back)
                z--;

            return z;
        }

        public float Horizontal()
        {
            float x = 0f;

            if (right)
                x++;

            if (left)
                x--;

            return x;
        }

        public Vector2 GetMouseInput() 
        {
            return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }

        public void Dispose() 
        {
            inputHandler.GetInputEvents(PlayerAction.Forward).OnChange -= OnForward;
            inputHandler.GetInputEvents(PlayerAction.Backward).OnChange -= OnBack;
            inputHandler.GetInputEvents(PlayerAction.Left).OnChange -= OnLeft;
            inputHandler.GetInputEvents(PlayerAction.Right).OnChange -= OnRight;
        }
    }
}