using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class InputManager : MonoBehaviour
    {
        public static event Action OnJumpClicked;
        public static event Action OnAttackClicked;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                OnJumpClicked?.Invoke();
            if (Input.GetKeyDown(KeyCode.E))
                OnJumpClicked?.Invoke();
        }
    }
}