using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Interactable.Resources;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public int PlayerId;
        
        [Header("Player Settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _factorAimingSpeed;
        [SerializeField] private int _timeBeforeCollecting;
        [SerializeField] private float _gravityScale;
        [SerializeField] private TriggerCollider _triggerCollider;
        [SerializeField] private WeaponHandler _revolverWeapon;
        [SerializeField] private WeaponHandler _rifleWeapon;
        [SerializeField] private List<Transform> _playerSpawnPoints;
        
        // Inventory
        private Dictionary<ResourcesEnum, int> _inventory = new Dictionary<ResourcesEnum, int>()
        {
            { ResourcesEnum.Stone, 200 },
            { ResourcesEnum.Metal, 200 },
            { ResourcesEnum.ElectricalCircuit, 200 }
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
        
        // Interactions
        private PadQte _actualPadQte;
        private ResourceHandler _lastInteractableResource;
        private TowerHandler _lastInteractableTower;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            _characterController = GetComponent<CharacterController>();

            _equippedWeapon = _revolverWeapon;
            _equippedWeapon.gameObject.SetActive(true);
        }

        private void Update()
        { 
            // Movement - Rotation
            Vector3 movement = new Vector3(_movement.x * _moveSpeed, _gravityScale, _movement.y * _moveSpeed);
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

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
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
                bool isQteFinished = _actualPadQte.CheckQte(readValue);

                if (isQteFinished)
                {
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
                if (_equippedWeapon.WeaponData._weaponTypesEnum == WeaponTypesEnum.Revolver)
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

                GameObject nearestInteractable = _triggerCollider.GetNearestObject();
                
                if (nearestInteractable && !_isInteracting)
                {
                    if (nearestInteractable.CompareTag("Resource"))
                    {
                        Debug.Log("Interact with a Resource");
                        
                        _lastInteractableResource = nearestInteractable.GetComponent<ResourceHandler>();
                        if (_lastInteractableResource.CurrentCapacity > 0)
                        {
                            StartCoroutine(TmeBeforeCollecting());
                        }
                        else
                        {
                            Debug.Log("Resource is empty.");
                        }
                    }
                    if (nearestInteractable.CompareTag("Tower"))
                    {
                        foreach (KeyValuePair<ResourcesEnum, int> resource in _inventory)
                        {
                            Debug.Log($"key: {resource.Key}, value: {resource.Value}");
                        }
                        Debug.Log("Interact with a Tower");
                        
                        _lastInteractableTower = nearestInteractable.GetComponent<TowerHandler>();
                        _inventory = _lastInteractableTower.ReceiveResources(_inventory);
                        
                        foreach (KeyValuePair<ResourcesEnum, int> resource in _inventory)
                        {
                            Debug.Log($"key: {resource.Key}, value: {resource.Value}");
                        }
                        
                    }
                    if (nearestInteractable.CompareTag("ReloadHeal"))
                    {
                        Debug.Log("Interact with a ReloadHeal");
                    }
                    if (nearestInteractable.CompareTag("PowerPlant"))
                    {
                        Debug.Log("Interact with a PowerPlant");
                    }
                    if (nearestInteractable.CompareTag("Shield"))
                    {
                        Debug.Log("Interact with a Shield");
                    }
                    if (nearestInteractable.CompareTag("Lobby"))
                    {
                        Debug.Log("Interact with a Lobby");
                        SceneManager.LoadScene(2);
                    }
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
        
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            transform.position = GameManager.SpawnPoints[PlayerId];
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
                    _actualPadQte = new PadQte(_lastInteractableResource.ResourcesICanCollect());
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
            (ResourcesEnum resource, int value) tupleResource = _lastInteractableResource.GetResources(_actualPadQte.Score);
            _inventory[tupleResource.resource] += tupleResource.value;
            foreach (KeyValuePair<ResourcesEnum, int> resource in _inventory)
            {
                Debug.Log($"key: {resource.Key}, value: {resource.Value}");
            }
        }
    }
}
