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
        public InputControls InputControls { get => _input; set => _input = value; }

        public static event Action OnJumpClicked;
        public static event Action OnAttackClicked;

        private float screenWidth;
        private float attackDelay = 0.35f;  // Adjust the attack delay as needed
        private float jumpDelay = 0.08f;  // Adjust the jump delay as needed
        private float attackTimer;
        private float jumpTimer;

        protected override void Awake()
        {
            base.Awake();

            _input = new InputControls();
            screenWidth = Screen.width;
        }

        private void Update()
        {
            /*if (!Application.isMobilePlatform)
            {
                _isPressingJumpButton = _input.Ingame.Jump.IsPressed();
                _isPressingAttackButton = _input.Ingame.Attack.IsPressed();
            }*/

            if (Input.touches.Length > 0)
            {
                Touch touch = Input.touches[0];

                if (touch.phase == UnityEngine.TouchPhase.Began)
                {
                    Vector2 screenPos = touch.position;

                    if (screenPos.x >= screenWidth / 2)
                    {
                        if (Time.time >= attackTimer)
                        {
                            Attack();
                            attackTimer = Time.time + attackDelay;
                        }
                    }

                    if (screenPos.x < screenWidth / 2)
                    {
                        if (Time.time >= jumpTimer)
                        {
                            Jump();
                            jumpTimer = Time.time + jumpDelay;
                        }
                    }
                }
                else if (touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    _isPressingJumpButton = false;
                }
            }
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Ingame.Jump.performed += (context) => Jump();
            _input.Ingame.Attack.performed += (context) => Attack();
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Ingame.Jump.performed -= (context) => Jump();
            _input.Ingame.Attack.performed -= (context) => Attack();
        }

        private void Jump()
        {
            OnJumpClicked?.Invoke();
            _isPressingJumpButton = true;
        }

        private void Attack()
        {
            OnAttackClicked?.Invoke();
        }

        private void TouchPress_started(InputAction.CallbackContext obj)
        {
            Vector2 touchPosition1 = _input.Ingame.TouchPosition1.ReadValue<Vector2>();
            Vector2 touchPosition2 = _input.Ingame.TouchPosition2.ReadValue<Vector2>();

            // Check on mobile if click was on left side of the screen
            if (touchPosition1.x <= Camera.main.scaledPixelWidth / 2
                || touchPosition2.x <= Camera.main.scaledPixelWidth / 2)
                OnJumpClicked?.Invoke();

            // Check on mobile if click was on right side of the screen
            if (touchPosition1.x >= Camera.main.scaledPixelWidth / 2
                || touchPosition2.x >= Camera.main.scaledPixelWidth / 2)
                OnAttackClicked?.Invoke();
        }
    }
}
