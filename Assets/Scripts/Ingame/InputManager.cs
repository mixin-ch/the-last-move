using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mixin.TheLastMove
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField]
        private InputControls _playerInput;

        private bool _isPressingJumpButton;
        private bool _isPressingAttackButton;

        public bool IsPressingJumpButton { get => _isPressingJumpButton; }
        public bool IsPressingAttackButton { get => _isPressingAttackButton; }

        public static event Action OnJumpClicked;
        public static event Action OnAttackClicked;

        protected override void Awake()
        {
            base.Awake();

            _playerInput = new InputControls();
        }

        private void Update()
        {
            _isPressingJumpButton = _playerInput.Ingame.Jump.IsPressed();
            _isPressingAttackButton = _playerInput.Ingame.Attack.IsPressed();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.Ingame.Jump.started += Jump_started;
            _playerInput.Ingame.Attack.started += Attack_started;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            _playerInput.Ingame.Jump.started -= Jump_started;
            _playerInput.Ingame.Attack.started -= Attack_started;
        }

        private void Jump_started(InputAction.CallbackContext obj)
        {
            OnJumpClicked?.Invoke();
        }

        private void Attack_started(InputAction.CallbackContext obj)
        {
            OnAttackClicked?.Invoke();
        }
    }
}
