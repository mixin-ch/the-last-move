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

            //Debug.Log(_input.Ingame.Jump.ReadValue<Vector2>());
            //Debug.Log(_input.Ingame.Jump.IsPressed());
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
            Vector2 touchPosition = _input.Ingame.TouchPosition.ReadValue<Vector2>();

            // Check on mobile if click was on left side of the screen
            if (Application.isMobilePlatform
                && touchPosition.x <= Camera.main.scaledPixelWidth / 2)
                OnJumpClicked?.Invoke();

            // Check on mobile if click was on right side of the screen
            if (Application.isMobilePlatform
                && touchPosition.x >= Camera.main.scaledPixelWidth / 2)
                OnAttackClicked?.Invoke();

            if (!Application.isMobilePlatform)
                OnJumpClicked?.Invoke();
        }

        private void Attack_started(InputAction.CallbackContext obj)
        {
            Vector2 touchPosition = _input.Ingame.TouchPosition.ReadValue<Vector2>();

            // Check on mobile if click was on right side of the screen
            if (Application.isMobilePlatform
                && touchPosition.x <= Camera.main.scaledPixelWidth / 2)
                return;

            OnAttackClicked?.Invoke();
        }
    }
}
