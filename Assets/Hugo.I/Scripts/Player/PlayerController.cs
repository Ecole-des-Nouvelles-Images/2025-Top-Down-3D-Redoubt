using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Displays.InGame_WorldSpace;
using Hugo.I.Scripts.Enemies;
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
    public class PlayerController : MonoBehaviour, IHaveHealth
    {
        public int PlayerId;
        
        [Header("Player Settings")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _increaseRateHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _factorAimingSpeed;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _pushDuration;
        [SerializeField] private int _timeBeforeCollecting;
        [SerializeField] private int _maxStone;
        [SerializeField] private int _maxMetal;
        [SerializeField] private int _maxCircuit;
        [SerializeField] private float _gravityScale;
        [SerializeField] private TriggerCollider _interactableTriggerCollider;
        [SerializeField] private TriggerCollider _repelTriggerCollider;
        [SerializeField] private WeaponHandler _revolverWeapon;
        [SerializeField] private WeaponHandler _rifleWeapon;
        
        [Header("Displays")]
        [SerializeField] private CanvasLookCameraHandler _canvasLookCameraHandler;
        [SerializeField] private PlayerWorldSpaceDisplay _playerWorldSpaceDisplay;
        [SerializeField] private PlayerWorldSpaceDisplayInteractions _playerWorldSpaceDisplayInteractions;

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
            { ResourcesEnum.Stone, 0 },
            { ResourcesEnum.Metal, 0 },
            { ResourcesEnum.ElectricalCircuit, 0 }
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
            
            CurrentHealth = _maxHealth;

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
                (int advancement, bool isCorrect, bool isFinished) tuple = _actualPadQte.CheckQte(readValue);
                _playerWorldSpaceDisplayInteractions.DisplayQteAdvancement(tuple.advancement, tuple.isCorrect);

                if (tuple.isFinished)
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
                        Debug.Log("Interact with a Tower");
                        
                        _lastInteractableTower = nearestInteractable.GetComponent<TowerHandler>();
                        _inventory = _lastInteractableTower.ReceiveResources(_inventory);
                        
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
                
                // Display
                _playerWorldSpaceDisplayInteractions.ResetInteractionButtonFill();
            }
        }

        public void OnPush(float readValue)
        {
            // Debug.Log("ButtonWest : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }

            if (readValue > 0)
            {
                List<GameObject> enemiesGameObjects = new List<GameObject>();
                enemiesGameObjects = _repelTriggerCollider.GetGameObjectsWithTag("Enemy");

                Debug.Log("Enemy pushed : " + enemiesGameObjects.Count);
                foreach (GameObject enemy in enemiesGameObjects)
                {
                    Vector3 direction = (enemy.transform.position - transform.position).normalized;
                    enemy.GetComponent<EnemyAIHandler>().IsPushed(direction, _pushForce, _pushDuration);
                } 
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

        public (float, float, float, float, Dictionary<ResourcesEnum, int>, int, int, int) GetCanvasHudData()
        {
            return (_maxHealth, CurrentHealth, _equippedWeapon.WeaponData.Capacity, _equippedWeapon.CurrentCapacity,
                _inventory, _maxStone, _maxMetal, _maxCircuit);
        }

        public (float, float, float, float) GetCanvasWorldSpaceData()
        {
            return (_maxHealth, CurrentHealth, _equippedWeapon.WeaponData.OverheatingLimit,
                _equippedWeapon.CurrentOverheating);
        }
        
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            transform.position = GameManager.SpawnPoints[PlayerId];
            _canvasLookCameraHandler.OnSceneLoaded();
            _playerWorldSpaceDisplayInteractions.HideInteractionsButton();
        }

        private IEnumerator TmeBeforeCollecting(string interactableName)
        {
            float time = 0f;
            while (_pressesButtonSouth)
            {
                time += 0.01f;
                _playerWorldSpaceDisplayInteractions.UpdateInteractionButtonFill(time, _timeBeforeCollecting);

                if (time >= _timeBeforeCollecting)
                {
                    _isInteracting = true;
                    _actualInteractableName = interactableName;

                    if (interactableName == "Resource")
                    {
                        (ResourcesEnum, int) resource = _lastInteractableResource.ResourcesICanCollect();
                        int size = 0;
                        
                        if (resource.Item1 == ResourcesEnum.Stone)
                        {
                            size = Mathf.Min(_maxStone - _inventory[resource.Item1], resource.Item2);
                        }
                        if (resource.Item1 == ResourcesEnum.Metal)
                        {
                            size = Mathf.Min(_maxMetal - _inventory[resource.Item1], resource.Item2);
                        }
                        if (resource.Item1 == ResourcesEnum.ElectricalCircuit)
                        {
                            size = Mathf.Min(_maxCircuit - _inventory[resource.Item1], resource.Item2);
                        }
                        
                        _actualPadQte = new PadQte(size);
                    }
                    if (interactableName == "PowerPlant")
                    {
                        _actualPadQte = new PadQte(_lastInteractablePowerPlant.QteSize);
                    }
                    
                    // Display
                    _playerWorldSpaceDisplayInteractions.HideInteractionsButton();
                    _playerWorldSpaceDisplayInteractions.DisplayQteButton(_actualPadQte.Qte);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        private void QuitQte()
        {
            _isInteracting = false;
            
            if (_actualInteractableName == "Resource")
            {
                (ResourcesEnum resource, int value) tupleResource = _lastInteractableResource.GetResources(_actualPadQte.Score);
                _inventory[tupleResource.resource] += tupleResource.value;
            }
            if (_actualInteractableName == "PowerPlant")
            {
                if (_actualPadQte.Score == _lastInteractablePowerPlant.QteSize)
                {
                    _lastInteractablePowerPlant.Repair();
                }
            }
            
            // Display
            _playerWorldSpaceDisplayInteractions.HideQteButton();
            _playerWorldSpaceDisplayInteractions.DisplayInteractionsButton();
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
        }
    }
}