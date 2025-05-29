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
    /// 
    /// you need to make this work for the new shit.
    /// 
    /// and then we add another alyer of abstraction onto this AGIAN !!! 
    /// 
    /// MAYBE MAKE THIS WORK WITH INTERFACE SO WE CAN ABSTRACT OUT CONTROLLERS AND SUCH???
    /// NAH ??
    /// </summary>
    public class KeyboardSource : IPlayerInput
    {
        //private readonly IInputService inputService;
        
        private readonly InputSource forward;
        private readonly InputSource back;
        private readonly InputSource left;
        private readonly InputSource right;

        public KeyboardSource()
        {
            IInputService inputService = ServiceLocator<IInputService>.Locate();

            forward = inputService.GetInputSource(PlayerAction.Forward);
            back = inputService.GetInputSource(PlayerAction.Backward);
            left = inputService.GetInputSource(PlayerAction.Left);
            right = inputService.GetInputSource(PlayerAction.Right);
        }

        /*private void OnForward(bool value) => forward = value;
        private void OnBack(bool value) => back = value;
        private void OnLeft(bool value) => left = value;
        private void OnRight(bool value) => right = value;*/

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

        /*public void Dispose() 
        {
            inputService.GetInputSource(PlayerAction.Forward).OnChange -= OnForward;
            inputService.GetInputSource(PlayerAction.Backward).OnChange -= OnBack;
            inputService.GetInputSource(PlayerAction.Left).OnChange -= OnLeft;
            inputService.GetInputSource(PlayerAction.Right).OnChange -= OnRight;
        }*/
    }
}