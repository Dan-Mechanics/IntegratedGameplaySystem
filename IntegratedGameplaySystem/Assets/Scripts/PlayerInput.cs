using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
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