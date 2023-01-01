using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class InputManager : Singleton<InputManager>
    {
        private KeyCode _jumpKey;
        private KeyCode _attackKey;

        private bool _isPressingJumpButton;
        private bool _isPressingAttackButton;

        public bool IsPressingJumpButton { get => _isPressingJumpButton; }
        public bool IsPressingAttackButton { get => _isPressingAttackButton; }

        public static event Action OnJumpClicked;
        public static event Action OnAttackClicked;

        private void Start()
        {
            _jumpKey = KeyCode.Space;
            _attackKey = KeyCode.E;
        }

        private void Update()
        {
            if (Input.GetKeyDown(_jumpKey))
                OnJumpClicked?.Invoke();
            if (Input.GetKeyDown(_attackKey))
                OnJumpClicked?.Invoke();

            _isPressingJumpButton = Input.GetKey(_jumpKey);
            _isPressingAttackButton = Input.GetKey(_attackKey);
        }
    }
}
