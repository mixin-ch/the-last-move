using Mixin.TheLastMove.Scene;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace Mixin.TheLastMove
{
    public class TutorialManager : MonoBehaviour
    {

        private int _page = -1;
        private float _screenWidth;

        [SerializeField]
        private InputSpriteStruct[] _inputSprites;
        private InputControls _inputControls;

        private void Awake()
        {
            _inputControls = new InputControls();
            _screenWidth = Screen.width;
        }


        // Start is called before the first frame update
        void Start()
        {
            _inputControls.Enable();
            ShowNextPage();
        }



        public void ShowNextPage()
        {
            _page++;
            if (_page < 2)
            {
                ChangeBackgroundImage(GetBackgroundSprite());
            }
            else
            {
                ChangeScene(SceneName.Ingame, LoadSceneMode.Single);
            }
        }
        public void ShowLastPage()
        {
            _page--;
            ChangeBackgroundImage(GetBackgroundSprite());
            if (_page < 0)
            {
                ChangeScene(SceneName.MainMenu, LoadSceneMode.Single);
            }
        }

        private void ChangeScene(SceneName sceneName, LoadSceneMode loadSceneMode)
        {
            SceneTransitionManager.Instance.LoadSceneWithTransition(sceneName, loadSceneMode);
        }

        private void ChangeBackgroundImage(Sprite sprite)
        {
            TutorialUIB.Instance.ChangeBackgroundImage(sprite);
        }

        private InputSpriteStruct GetCurrentInputSprites()
        {
            InputDeviceType last = InputDeviceType.Unknown;
            double time = 0;
            _inputControls = new InputControls();
            foreach (InputDevice d in InputSystem.devices)
            {
                if (d.lastUpdateTime >= time)
                {
                    last = GetInputDeviceType(d);
                    time = d.lastUpdateTime;
                }
            }
            foreach (InputSpriteStruct iss in _inputSprites)
            {
                foreach (InputDeviceType type in iss.inputDevices)
                {
                    if (last == type)
                    {
                        return iss;
                    }
                }

            }
            return new InputSpriteStruct();

        }

        private Sprite GetBackgroundSprite()
        { 
            InputSpriteStruct inSprites = GetCurrentInputSprites();
            switch (_page)
            {
                case 0:
                    return inSprites.jump;
                case 1:
                    return inSprites.attack;
                default:
                    return null;
            }
        }

        // Update is called once per frame
        void Update() {
        
            if (Input.touches.Length > 0)
            {
                for (int i = 0; i<Input.touches.Length; i++)
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
                        if (touch.position.x<_screenWidth / 2)
                        {
                            Jump();
                        }
                    }
                }
            }
        }


        private void OnEnable()
        {
            _inputControls.Ingame.Jump.performed += JumpWrapper;
            _inputControls.Ingame.Attack.performed += AttackWrapper;
        }

        private void OnDisable()
        {
            _inputControls.Ingame.Jump.performed -= JumpWrapper;
            _inputControls.Ingame.Attack.performed -= AttackWrapper;
            _inputControls.Disable();
        }

        private void JumpWrapper(InputAction.CallbackContext context)
        {
            Jump();
        }

        private void AttackWrapper(InputAction.CallbackContext context)
        {
            Attack();
        }

        private void Jump()
        {
            if(_page == 0)
            {
                ShowNextPage();
            }
        }
        private void Attack()
        {
            if(_page == 1)
            {
                ShowNextPage();
            }
        }

        private InputDeviceType GetInputDeviceType(InputDevice ind)
        {
            if (ind is Keyboard)
            {
                return InputDeviceType.Keyboard;
            }
            else if (ind is Mouse)
            {
                return InputDeviceType.Mouse;
            }
            else if(ind is Gamepad)
            {
                return InputDeviceType.Gamepad;
            }
            else if (ind is Touchscreen)
            {
                return InputDeviceType.Touchscreen;
            }
            else
            {
                return InputDeviceType.Unknown;
            }
        }

    }

   

    [Serializable]
    public struct InputSpriteStruct
    {
        public InputDeviceType[] inputDevices;
        public Sprite jump;
        public Sprite attack;
    }

    public enum InputDeviceType
    {
        Unknown,
        Keyboard,
        Mouse,
        Gamepad,
        Touchscreen
    }


}