using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Displays.InGame_WorldSpace;
using Hugo.I.Scripts.Enemies;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Interactable.PowerPlant;
using Hugo.I.Scripts.Interactable.Resources;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Shield;
using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        [SerializeField] private GameObject _upperBodyAimGameObject;
        [SerializeField] private float _factorAimingSpeed;
        [SerializeField] private float _factorCarryingSpeed;
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
        [SerializeField] private Transform _carrieShieldTransform;
        [SerializeField] private Animator _animator;
        
        [FormerlySerializedAs("_canvasLookSizeCameraHandler")]
        [FormerlySerializedAs("_canvasLookCameraHandler")]
        [Header("Displays")]
        [SerializeField] private CanvasHandler _canvasHandler;
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
                    Die();
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
        private PlayerInputHandler _playerInputHandler;
        
        // States
        private bool _isShooting;
        private bool _isAiming;
        private bool _isInteracting;
        private bool _pressesButtonSouth;
        private bool _wantToReload;
        private bool _wantToHeal;
        private bool _isCarrying;
        private bool _isDead;
        
        // Movements - Rotations
        private Vector2 _movement;
        private Vector2 _lastMovementBeforeZero;
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
        private ShieldHandler _lastInteractableShield;
        
        // Animator
        private Vector3 _previousPosition;
        public Vector3 Velocity { get; private set; }
        public float SignedForwardSpeed { get; private set; }
        public float SignedRightSpeed { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            _characterController = GetComponent<CharacterController>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            
            CurrentHealth = _maxHealth;

            _equippedWeapon = _revolverWeapon;
            _equippedWeapon.gameObject.SetActive(true);
        }

        private void Start()
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            _previousPosition = transform.position;
        }

        private void Update()
        {
            if (_isDead) return;
            
            // Movement - Rotation
            Vector3 movement = new Vector3(_movement.x, _gravityScale, _movement.y);

            if (_playerInputHandler.InputAreEnable == false)
            {
                movement = Vector3.zero;
            }
            
            if (_isCarrying)
            {
                movement *= _factorCarryingSpeed;
            }
                
            var upperBodyAimDirection = new Vector3(_lastMovementBeforeZero.x, 0f, _lastMovementBeforeZero.y).normalized;
            var targetRotation = Quaternion.LookRotation(new Vector3(_lastMovementBeforeZero.x, 0, _lastMovementBeforeZero.y));
            
            if (_isAiming)
            {
                movement *= _factorAimingSpeed;

                upperBodyAimDirection = new Vector3(_aiming.x, 0f, _aiming.y).normalized;

                if (_movement != Vector2.zero)
                {
                    if (Vector2.Dot(_movement, _aiming)  < 0f)
                    {
                        targetRotation = Quaternion.LookRotation(new Vector3(_aiming.x, 0, _aiming.y));
                    }
                }
                else
                {
                    Debug.Log("PLayer immobile");
                    Vector2 lookDirection = new Vector2(transform.forward.x, transform.forward.y).normalized;

                    if (Vector2.Dot(lookDirection, _aiming.normalized)  < 0f)
                    {
                        Debug.Log("Test");
                        targetRotation = Quaternion.LookRotation(new Vector3(_aiming.x, 0, _aiming.y));
                    }
                }
            }
            
            _characterController.Move(movement * (_moveSpeed * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            _upperBodyAimGameObject.transform.position = transform.position + upperBodyAimDirection * 1f;
            
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
            
            // Animator Speed
            Vector3 displacement = transform.position - _previousPosition;
            Velocity = displacement / Time.deltaTime;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            SignedForwardSpeed = Vector3.Dot(Velocity, forward);
            SignedRightSpeed = Vector3.Dot(Velocity, right);

            _previousPosition = transform.position;
            
            // Animator
            // Debug.Log(SignedForwardSpeed + " / " + SignedRightSpeed);
            _animator.SetFloat("SpeedX", SignedForwardSpeed);
            _animator.SetFloat("SpeedY", SignedRightSpeed);
            _animator.SetBool("IsAiming", _isAiming);
            _animator.SetBool("IsShooting", _isShooting);
            _animator.SetBool("IsInteracting", _isInteracting);
            _animator.SetBool("IsDead", _isDead);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnMove(Vector2 readValue)
        {
            if (_isDead) return;
            
            // Debug.Log("Left Joystick : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }
            
            _movement = readValue;
            
            if (readValue != Vector2.zero)
            {
                _lastMovementBeforeZero = readValue;
            }
        }

        public void OnAim(Vector2 readValue)
        {
            if (_isDead || _isCarrying) return;
            
            // Debug.Log("Right Joystick : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }
            
            _aiming = readValue;
            
            if (readValue != Vector2.zero)
            {
                _isAiming = true;
            }
            else
            {
                _isAiming = false;
            }
        }
        
        public void OnQte(Vector2 readValue)
        {
            if (_isDead || _isCarrying) return;
            
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
            if (_isDead || _isCarrying) return;
            
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
                
                // Animator
                _animator.SetTrigger("SwitchWeapon");
            }
        }

        public void OnInteract(float readValue)
        {
            if (_isDead) return;
            
            // Debug.Log("ButtonSouth : " + readValue);
            
            if (readValue > 0)
            {
                _pressesButtonSouth = true;

                if (_isCarrying)
                {
                    List<GameObject> towersGameObjects = _interactableTriggerCollider.GetGameObjectsWithTag("Tower");

                    if (towersGameObjects.Count > 0)
                    {
                        _isCarrying = false;
                        towersGameObjects[0].GetComponent<TowerHandler>().ReceiveShield();
                        _lastInteractableShield.Disappears();
                        return;
                    }
                }

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

                        _lastInteractableShield = nearestInteractable.GetComponent<ShieldHandler>();

                        if (!_isCarrying)
                        {
                            _isCarrying = true;
                            _lastInteractableShield.Carrie(_carrieShieldTransform);
                            _playerWorldSpaceDisplayInteractions.HideInteractionsButton();
                        }
                        else
                        {
                            _isCarrying = false;
                            _lastInteractableShield.Drop();
                        }
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
            if (_isDead || _isCarrying) return;
            
            // Debug.Log("ButtonWest : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }

            if (readValue > 0)
            {
                _playerInputHandler.InputAreEnable = false;
                
                List<GameObject> enemiesGameObjects = new List<GameObject>();
                enemiesGameObjects = _repelTriggerCollider.GetGameObjectsWithTag("Enemy");

                Debug.Log("Enemy pushed : " + enemiesGameObjects.Count);
                foreach (GameObject enemy in enemiesGameObjects)
                {
                    Vector3 direction = (enemy.transform.position - transform.position).normalized;
                    enemy.GetComponent<EnemyAIHandler>().IsPushed(direction, _pushForce, _pushDuration);
                } 
                
                StartCoroutine(SetEnableInputs(true, _pushDuration));
                
                // Animator
                _animator.SetTrigger("Push");
            }
        }

        public void OnShoot(float readValue)
        {
            if (_isDead || _isCarrying) return;
            
            // Debug.Log("R2 : " + readValue);
            if (_isInteracting)
            {
                QuitQte();
            }

            if (Mathf.Approximately(readValue, 1))
            {
                _isShooting = true;
            }
            else
            {
                _isShooting = false;
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
            if (arg0.buildIndex == 1)
            {
                // transform.position = GameManager.Instance.SpawnPointsLobby[PlayerId];
                
            }
            else if (arg0.buildIndex == 2)
            {
                // transform.position = GameManager.Instance.SpawnPointsInGame[PlayerId];
            }
            
            _playerInputHandler.InputAreEnable = true;
            _canvasHandler.OnSceneLoaded();
            _playerWorldSpaceDisplayInteractions.HideInteractionsButton();
            
            _inventory[ResourcesEnum.Stone] = 200;
            _inventory[ResourcesEnum.Metal] = 200;
            _inventory[ResourcesEnum.ElectricalCircuit] = 200;
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

        private IEnumerator SetEnableInputs(bool enable, float duration)
        {
            yield return new WaitForSeconds(duration);
            _playerInputHandler.InputAreEnable = enable;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;
            
            CurrentHealth -= damage;
            
            // Animator
            _animator.SetTrigger("TakeDamage");
        }

        private void Die()
        {
            _isDead = true;
            GameManager.Instance.APlayerDie(gameObject);
            Destroy(gameObject, 2f);
        }
    }
}