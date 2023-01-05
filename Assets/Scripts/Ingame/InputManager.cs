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
        private InputControls _input;

        private bool _isPressingJumpButton;
        private bool _isPressingAttackButton;

        public bool IsPressingJumpButton { get => _isPressingJumpButton; }
        public bool IsPressingAttackButton { get => _isPressingAttackButton; }
        public InputControls Input { get => _input; set => _input = value; }

        public static event Action OnJumpClicked;
        public static event Action OnAttackClicked;

        protected override void Awake()
        {
            base.Awake();

            _input = new InputControls();
        }

        private void Update()
        {
            _isPressingJumpButton = _input.Ingame.Jump.IsPressed();
            _isPressingAttackButton = _input.Ingame.Attack.IsPressed();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Ingame.Jump.started += Jump_started;
            _input.Ingame.Attack.started += Attack_started;
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Ingame.Jump.started -= Jump_started;
            _input.Ingame.Attack.started -= Attack_started;
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
