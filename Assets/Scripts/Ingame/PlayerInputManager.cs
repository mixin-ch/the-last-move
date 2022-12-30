using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static event Action OnJumpClicked;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                OnJumpClicked?.Invoke();
        }
    }
}
