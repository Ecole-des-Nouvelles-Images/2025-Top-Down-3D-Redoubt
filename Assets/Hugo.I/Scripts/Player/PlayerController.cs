using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Interactable.PowerPlant;
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
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _increaseRateHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _factorAimingSpeed;
        [SerializeField] private int _timeBeforeCollecting;
        [SerializeField] private float _gravityScale;
        [SerializeField] private TriggerCollider _interactableTriggerCollider;
        [SerializeField] private TriggerCollider _repelTriggerCollider;
        [SerializeField] private WeaponHandler _revolverWeapon;
        [SerializeField] private WeaponHandler _rifleWeapon;

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (Mathf.Approximately(_currentHealth, _maxHealth))
                {
                    _wantToHeal = false;
                }
                if (_currentHealth <= 0)
                {
                    // Joueur tombe au sol
                }
            }
        }
        
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
        private bool _wantToReload;
        private bool _wantToHeal;
        
        // Movements - Rotations
        private Vector2 _movement;
        private Vector2 _nonNullAim;
        private Vector2 _aiming;
        
        // Weapons
        private WeaponHandler _equippedWeapon;
        
        // Interactions
        private PadQte _actualPadQte;
        private string _actualInteractableName;
        private ResourceHandler _lastInteractableResource;
        private TowerHandler _lastInteractableTower;
        private PowerPlantHandler _lastInteractablePowerPlant;
        private ReloadHealingHandler _lastInteractableReloadHealing;

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
            
            // Reload - Heal
            if (_wantToReload && _equippedWeapon.CurrentCapacity < _equippedWeapon.WeaponData.Capacity)
            {
                if (_lastInteractableReloadHealing.UseEnergy())
                {
                    _equippedWeapon.Reload();
                }
            }

            if (_wantToHeal)
            {
                if (_lastInteractableReloadHealing.UseEnergy())
                {
                    CurrentHealth += _increaseRateHealth * Time.deltaTime;;
                }
            }
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

                GameObject nearestInteractable = _interactableTriggerCollider.GetNearestObject();
                if (nearestInteractable)
                {
                    Debug.Log("Nearest interactable : " + nearestInteractable.name + " / " + nearestInteractable.tag);
                }
                
                if (nearestInteractable && !_isInteracting)
                {
                    if (nearestInteractable.CompareTag("Resource"))
                    {
                        Debug.Log("Interact with a Resource");
                        
                        _lastInteractableResource = nearestInteractable.GetComponent<ResourceHandler>();
                        if (_lastInteractableResource.CurrentCapacity > 0)
                        {
                            StartCoroutine(TmeBeforeCollecting("Resource"));
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
                    if (nearestInteractable.CompareTag("Reload"))
                    {
                        Debug.Log("Interact with a Reload");
                        _lastInteractableReloadHealing = nearestInteractable.GetComponent<ReloadHealingHandler>();
                        _wantToReload = true;
                    }
                    if (nearestInteractable.CompareTag("Heal"))
                    {
                        Debug.Log("Interact with a Heal");
                        _lastInteractableReloadHealing = nearestInteractable.GetComponent<ReloadHealingHandler>();
                        _wantToHeal = true;
                    }
                    if (nearestInteractable.CompareTag("PowerPlant"))
                    {
                        Debug.Log("Interact with a PowerPlant");
                        
                        _lastInteractablePowerPlant = nearestInteractable.GetComponent<PowerPlantHandler>();
                        
                        if (!_lastInteractablePowerPlant.IsRepaired)
                        {
                            StartCoroutine(TmeBeforeCollecting("PowerPlant"));
                        }
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
                _wantToReload = false;
                _wantToHeal = false;
            }
        }

        public void OnRepel(float readValue)
        {
            // Debug.Log("ButtonWest : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }

            List<GameObject> enemiesGameObjects = new List<GameObject>();
            enemiesGameObjects = _repelTriggerCollider.GetGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemiesGameObjects)
            {
                // lance la methode dans les ennemis directement qui vas les pousser
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

        private IEnumerator TmeBeforeCollecting(string interactableName)
        {
            int time = 0;
            while (_pressesButtonSouth)
            {
                time++;

                if (time >= _timeBeforeCollecting)
                {
                    _isInteracting = true;
                    _actualInteractableName = interactableName;

                    if (interactableName == "Resource")
                    {
                        _actualPadQte = new PadQte(_lastInteractableResource.ResourcesICanCollect());
                    }
                    if (interactableName == "PowerPlant")
                    {
                        _actualPadQte = new PadQte(_lastInteractablePowerPlant.QteSize);
                    }
                    
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
            
            if (_actualInteractableName == "Resource")
            {
                (ResourcesEnum resource, int value) tupleResource = _lastInteractableResource.GetResources(_actualPadQte.Score);
                _inventory[tupleResource.resource] += tupleResource.value;
                foreach (KeyValuePair<ResourcesEnum, int> resource in _inventory)
                {
                    Debug.Log($"key: {resource.Key}, value: {resource.Value}");
                }
            }
            if (_actualInteractableName == "PowerPlant")
            {
                if (_actualPadQte.Score == _lastInteractablePowerPlant.QteSize)
                {
                    _lastInteractablePowerPlant.Repair();
                }
            }
        }
    }
}