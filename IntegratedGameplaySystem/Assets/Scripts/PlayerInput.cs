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
        /// <summary>
        /// Or you could cahce ??
        /// </summary>
        public Vector3 GetMovementDirection() 
        {
            return new Vector3(
                Input.GetAxisRaw("Horizontal"),

                (Input.GetKey(KeyCode.Space) ? 1f : 0f) +
                (Input.GetKey(KeyCode.LeftShift) ? -1f : 0f),

                Input.GetAxisRaw("Vertical"));;
        }

        /// <summary>
        /// Or you could cahce ??
        /// </summary>
        public Vector2 GetMouseInput() 
        {
            return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
    }
}