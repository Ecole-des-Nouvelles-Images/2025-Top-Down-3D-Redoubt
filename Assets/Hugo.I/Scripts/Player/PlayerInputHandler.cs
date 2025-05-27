using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.I.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public static event Action<bool> OnInputDeviceChanged;
        
        private PlayerInput _playerInput;
        
        private PlayerController _playerController;
        
        private bool _isControllerConnected;
        
        private Vector2 _joystickReadValue;
        private float _westButtonReadValue;
        private float _leftButtonReadValue;

        public bool InputAreEnable = true;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChange;

            // Bind input actions
            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMove;
            _playerInput.actions["Aim"].performed += OnAim;
            _playerInput.actions["Aim"].canceled += OnAim;
            _playerInput.actions["Qte"].performed += OnQte;
            _playerInput.actions["Qte"].canceled += OnQte;
            _playerInput.actions["SwitchWeapon"].performed += OnSwitchWeapon;
            _playerInput.actions["SwitchWeapon"].canceled += OnSwitchWeapon;
            _playerInput.actions["Interact"].performed += OnInteract;
            _playerInput.actions["Interact"].canceled += OnInteract;
            _playerInput.actions["Repel"].performed += OnPush;
            _playerInput.actions["Repel"].canceled += OnPush;
            _playerInput.actions["Shoot"].performed += OnShoot;
            _playerInput.actions["Shoot"].canceled += OnShoot;
            _playerInput.actions["Start"].performed += OnStart;
            _playerInput.actions["Start"].canceled += OnStart;
        }
        
        private void OnDisable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange -= OnDeviceChange;

            // Unbind input actions
            _playerInput.actions["Move"].performed -= OnMove;
            _playerInput.actions["Move"].canceled -= OnMove;
            _playerInput.actions["Aim"].performed -= OnAim;
            _playerInput.actions["Aim"].canceled -= OnAim;
            _playerInput.actions["Qte"].performed -= OnQte;
            _playerInput.actions["Qte"].canceled -= OnQte;
            _playerInput.actions["SwitchWeapon"].performed -= OnSwitchWeapon;
            _playerInput.actions["SwitchWeapon"].canceled -= OnSwitchWeapon;
            _playerInput.actions["Interact"].performed -= OnInteract;
            _playerInput.actions["Interact"].canceled -= OnInteract;
            _playerInput.actions["Repel"].performed -= OnPush;
            _playerInput.actions["Repel"].canceled -= OnPush;
            _playerInput.actions["Shoot"].performed -= OnShoot;
            _playerInput.actions["Shoot"].canceled -= OnShoot;
            _playerInput.actions["Start"].performed -= OnStart;
            _playerInput.actions["Start"].canceled -= OnStart;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed) DetectCurrentInputDevice();
        }

        private void DetectCurrentInputDevice()
        {
            _isControllerConnected = Gamepad.all.Count > 0;
            OnInputDeviceChanged?.Invoke(_isControllerConnected);

            Debug.Log(_isControllerConnected
                ? "Controller connected: Switching to Gamepad controls."
                : "No controller connected: Switching to Keyboard/Mouse controls.");
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnMove(context.ReadValue<Vector2>());
            }
        }
        
        private void OnAim(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnAim(context.ReadValue<Vector2>());
            }
        }
        
        private void OnQte(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnQte(context.ReadValue<Vector2>());
            }
        }
        
        private void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnSwitchWeapon(context.ReadValue<float>());
            }
        }
        
        private void OnInteract(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnInteract(context.ReadValue<float>());
            }
        }
        
        private void OnPush(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnPush(context.ReadValue<float>());
            }
        }
        
        private void OnShoot(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnShoot(context.ReadValue<float>());
            }
        }
        
        private void OnStart(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _playerController.OnStart(context.ReadValue<float>());
            }
        }
    }
}
