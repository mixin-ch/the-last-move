using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Player;
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

        private float _screenWidth;

        [SerializeField]
        private float _attackDelay = 0.35f;  // Adjust the attack delay as needed

        [SerializeField]
        private float _jumpDelay = 0.1f;  // Adjust the jump delay as needed

        private float _attackTimer;

        private float _jumpTimer;

        public bool IsPressingJumpButton { get => _isPressingJumpButton; }
        public InputControls InputControls { get => _input; set => _input = value; }

        public static event Action OnPlayerJump;
        public static event Action OnPlayerAttack;

        protected override void Awake()
        {
            base.Awake();

            _input = new InputControls();
            _screenWidth = Screen.width;
        }

        private void Update()
        {
            if (!EnvironmentManager.Instance.IsGameRunning)
                return;

            if (_input.Ingame.Jump.WasReleasedThisFrame())
                _isPressingJumpButton = false;

            if (Input.touches.Length > 0)
            {
                for (int i = 0; i < Input.touches.Length; i++)
                {
                    Touch touch = Input.touches[i];

                    // Attack
                    if (touch.position.x >= _screenWidth / 2)
                    {
                        Attack();
                    }

                    // Jump
                    if (touch.phase == UnityEngine.TouchPhase.Began)
                    {
                        if (touch.position.x < _screenWidth / 2)
                        {
                            Jump();
                        }
                    }
                    else if (touch.phase == UnityEngine.TouchPhase.Ended)
                    {
                        if (touch.position.x < _screenWidth / 2)
                        {
                            _isPressingJumpButton = false;
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            _input.Ingame.Jump.performed += (context) => Jump();
            _input.Ingame.Attack.performed += (context) => Attack();
            PlayerOperator.OnPlayerDeathEvent += (_) => PlayerOperator_OnPlayerDeathEvent();
            EnvironmentManager.OnGameStarted += EnvironmentManager_OnGameStarted;
        }

        private void OnDisable()
        {
            _input.Ingame.Jump.performed -= (context) => Jump();
            _input.Ingame.Attack.performed -= (context) => Attack();
            PlayerOperator.OnPlayerDeathEvent -= (_) => PlayerOperator_OnPlayerDeathEvent();
            EnvironmentManager.OnGameStarted -= EnvironmentManager_OnGameStarted;
        }

        private void Jump()
        {
            if (!EnvironmentManager.Instance.PlayerOperator.CanJump)
                return;

            if (Time.time >= _jumpTimer)
            {
                // Code to handle Jump action
                _jumpTimer = Time.time + _jumpDelay;
                OnPlayerJump?.Invoke();
                _isPressingJumpButton = true;
            }
        }

        private void Attack()
        {
            if (Time.time >= _attackTimer)
            {
                // Code to handle Attack action
                _attackTimer = Time.time + _attackDelay;
                OnPlayerAttack?.Invoke();
            }
        }

        private void EnvironmentManager_OnGameStarted()
        {
            _input.Enable();
        }

        private void PlayerOperator_OnPlayerDeathEvent()
        {
            _input.Disable();
        }

    }
}
