using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using UnityEngine;
using Resources = Hugo.I.Scripts.Utils.Resources;

namespace Hugo.I.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _factorAimingSpeed;
        [SerializeField] private int _timeBeforeCollecting;
        [SerializeField] private TriggerCollider _triggerCollider;
        [SerializeField] private WeaponHandler _revolverWeapon;
        [SerializeField] private WeaponHandler _rifleWeapon;
        
        // Inventory
        Dictionary<Resources, int> _inventory = new Dictionary<Resources, int>()
        {
            { Resources.Stone, 0 },
            { Resources.Metal, 0 },
            { Resources.ElectricalCircuit, 0 }
        };
        
        // Internals Components
        private CharacterController _characterController;
        
        // States
        private bool _isInteracting;
        private bool _pressesButtonSouth;
        
        // Movements - Rotations
        private Vector2 _movement;
        private Vector2 _nonNullAim;
        private Vector2 _aiming;
        
        // Weapons
        private WeaponHandler _equippedWeapon;
        
        // QTE
        private PadQte _actualPadQte;
        private IResource _interactResource;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            _equippedWeapon = _revolverWeapon;
            _equippedWeapon.gameObject.SetActive(true);
        }

        private void Update()
        {
            // Movement - Rotation
            Vector3 movement = new Vector3(_movement.x * _moveSpeed, 0, _movement.y * _moveSpeed);
            float angle;
            
            if (_aiming == Vector2.zero)
            {
                _characterController.Move(movement * Time.deltaTime);
                
                angle = Mathf.Atan2(_nonNullAim.x, _nonNullAim.y) * Mathf.Rad2Deg;
            }
            else
            {
                _characterController.Move(movement * _factorAimingSpeed * Time.deltaTime);
                
                angle = Mathf.Atan2(_aiming.x, _aiming.y) * Mathf.Rad2Deg;
            }
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        public void OnMove(Vector2 readValue)
        {
            // Debug.Log("Left Joystick : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }
            
            _movement = readValue;
            if (readValue != Vector2.zero)
            {
                _nonNullAim = readValue;
            }
        }

        public void OnAim(Vector2 readValue)
        {
            // Debug.Log("Right Joystick : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }
            
            _aiming = readValue;
            if (readValue != Vector2.zero)
            {
                _nonNullAim = readValue;
            }
        }
        
        public void OnQte(Vector2 readValue)
        {
            // Debug.Log("Pad : " + readValue);
            if (_isInteracting && readValue != Vector2.zero)
            {
                Debug.Log("Pad : " + readValue);
                (bool, int) resultQte = _actualPadQte.CheckQte(readValue);

                if (resultQte.Item1)
                {
                    _isInteracting = false;
                    _inventory[_interactResource.GetResourceType()] += resultQte.Item2;
                    QuitQte();
                }
            }
        }
        
        public void OnSwitchWeapon(float readValue)
        {
            // Debug.Log("ButtonNorth : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }

            if (readValue > 0)
            {
                if (_equippedWeapon.WeaponData.WeaponType == WeaponType.Revolver)
                {
                    _equippedWeapon.gameObject.SetActive(false);
                    _equippedWeapon = _rifleWeapon;
                    _equippedWeapon.gameObject.SetActive(true);
                }
                else
                {
                    _equippedWeapon.gameObject.SetActive(false);
                    _equippedWeapon = _revolverWeapon;
                    _equippedWeapon.gameObject.SetActive(true);
                }
            }
        }

        public void OnInteract(float readValue)
        {
            // Debug.Log("ButtonSouth : " + readValue);
            
            if (readValue > 0)
            {
                _pressesButtonSouth = true;
                if (_triggerCollider.GetNearestObject("Resource") && !_isInteracting)
                {
                    Debug.Log("Start Interact");
                    _interactResource = _triggerCollider.GetNearestObject("Resource").GetComponent<IResource>();
                    StartCoroutine(TmeBeforeCollecting());
                }
            }
            else
            {
                _pressesButtonSouth = false;
            }
        }

        public void OnRepel(float readValue)
        {
            // Debug.Log("ButtonWest : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }
        }

        public void OnShoot(float readValue)
        {
            // Debug.Log("R2 : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }
            
            _equippedWeapon.Shoot(readValue);
        }

        private IEnumerator TmeBeforeCollecting()
        {
            int time = 0;
            while (_pressesButtonSouth)
            {
                time++;

                if (time >= _timeBeforeCollecting)
                {
                    _isInteracting = true;
                    _actualPadQte = new PadQte(_interactResource.GetResourceMaxCollectable());
                    foreach (Vector2 vector2 in _actualPadQte.Qte)
                    {
                        Debug.Log(vector2);
                    }
                    yield break;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        private void QuitQte()
        {
            _isInteracting = false;
            _inventory[_interactResource.GetResourceType()] += _actualPadQte.Score;
            foreach (KeyValuePair<Resources, int> resource in _inventory)
            {
                Debug.Log($"key: {resource.Key}, value: {resource.Value}");
            }
        }
    }
}
