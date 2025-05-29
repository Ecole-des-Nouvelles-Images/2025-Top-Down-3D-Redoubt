using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Enemies;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Interactable.PowerPlant;
using Hugo.I.Scripts.Interactable.Resources;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Shield;
using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IHaveHealth
    {
        [Header("<size=13><color=#58D68D>Player Data</color></size>")]
        [SerializeField] private PlayerData _playerData;
        
        [Header("<size=13><color=#58D68D>References</color></size>")]
        [SerializeField] private Animator _animator;
        
        // Player Events
        public PlayerEvents Events { get; private set; } = new PlayerEvents();
        
        // Internals Components
        private CharacterController _characterController;
        private PlayerInputHandler _playerInputHandler;
        private PlayerTwoBonesIkHandler _playerTwoBonesIkHandler;
        
        // Movements - Rotations
        private Vector2 _movement;
        private Vector2 _lastMovementBeforeZero;
        private Vector2 _aiming;
        
        // Animator
        private Vector3 _previousPosition;
        private Vector3 _velocity { get; set; }
        private float _signedForwardSpeed { get; set; }
        private float _signedRightSpeed { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            _characterController = GetComponent<CharacterController>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _playerTwoBonesIkHandler = GetComponent<PlayerTwoBonesIkHandler>();
            
            _playerData.CurrentHealth = _playerData.PlayerBaseStats.MaxHealth;

            _playerData.EquippedWeapon = _playerData.RevolverWeapon;
            _playerData.EquippedWeapon.gameObject.SetActive(true);
        }

        private void Start()
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            _previousPosition = transform.position;
        }

        private void Update()
        {
            if (_playerData.IsDead) return;
            
            // Movement - Rotation
            Vector3 movement = new Vector3(_movement.x, _playerData.PlayerBaseStats.GravityScale, _movement.y);

            if (_playerInputHandler.InputAreEnable == false)
            {
                movement = Vector3.zero;
            }
            
            if (_playerData.IsCarrying)
            {
                movement *= _playerData.PlayerBaseStats.FactorCarryingSpeed;
            }
                
            var upperBodyAimDirection = new Vector3(_lastMovementBeforeZero.x, 0f, _lastMovementBeforeZero.y).normalized;
            var targetRotation = Quaternion.LookRotation(new Vector3(_lastMovementBeforeZero.x, 0, _lastMovementBeforeZero.y));
            
            if (_playerData.IsAiming)
            {
                movement *= _playerData.PlayerBaseStats.FactorAimingSpeed;

                upperBodyAimDirection = new Vector3(_aiming.x, 0f, _aiming.y).normalized;

                if (_movement != Vector2.zero)
                {
                    if (Vector2.Dot(_movement, _aiming)  < 0.7071f)
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
            
            _characterController.Move(movement * (_playerData.PlayerBaseStats.MoveSpeed * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            _playerData.UpperBodyAimGameObject.transform.position = transform.position + new Vector3(upperBodyAimDirection.x, upperBodyAimDirection.y + 1f, upperBodyAimDirection.z);
            
            // Reload - Heal
            if (_playerData.WantToReload && _playerData.EquippedWeapon.CurrentCapacity < _playerData.EquippedWeapon.WeaponData.Capacity)
            {
                if (_playerData.LastInteractableReloadHealing.UseEnergy())
                {
                    _playerData.EquippedWeapon.Reload();
                    
                    // Event
                    Events.Reloading(true);
                }
            }

            if (_playerData.WantToHeal)
            {
                if (_playerData.LastInteractableReloadHealing.UseEnergy())
                {
                    _playerData.CurrentHealth += _playerData.PlayerBaseStats.IncreaseRateHealth * Time.deltaTime;
                    
                    // Event
                    Events.Healing(true);
                }
            }
            
            // Animator Speed
            Vector3 displacement = transform.position - _previousPosition;
            _velocity = displacement / Time.deltaTime;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            
            float vertical = Vector3.Dot(_velocity.normalized, forward) * _velocity.magnitude;
            float horizontal = Vector3.Dot(_velocity.normalized, right) * _velocity.magnitude;
            if (vertical <= -1f)
            {
                horizontal = -horizontal;
            }
            
            float speed = Mathf.Clamp01(_velocity.magnitude);
            
            float normalizedVertical = vertical / (speed > 0 ? speed : 1f);
            float normalizedHorizontal = horizontal / (speed > 0 ? speed : 1f);
            
            if (!_playerData.IsAiming)
            {
                normalizedHorizontal = 0f;
            }
            
            // Animator
            _animator.SetFloat("SpeedX", Mathf.Clamp(normalizedVertical, -1f, 1f));
            _animator.SetFloat("SpeedY", Mathf.Clamp(normalizedHorizontal, -1f, 1f));
            _animator.SetBool("IsAiming", _playerData.IsAiming);
            _animator.SetBool("IsShooting", _playerData.IsShooting);
            _animator.SetBool("IsInteracting", _playerData.IsInteracting);
            
            // Events
            Events.Move(_characterController.velocity.magnitude);
            
            _previousPosition = transform.position;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnMove(Vector2 readValue)
        {
            if (_playerData.IsDead) return;
            
            // Debug.Log("Left Joystick : " + readValue);
            if (_playerData.IsInteracting)
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
            if (_playerData.IsDead || _playerData.IsCarrying) return;
            
            // Debug.Log("Right Joystick : " + readValue);
            if (_playerData.IsInteracting)
            {
                QuitQte();
            }
            
            _aiming = readValue;
            
            if (readValue != Vector2.zero)
            {
                _playerData.IsAiming = true;
            }
            else
            {
                _playerData.IsAiming = false;
            }
        }
        
        public void OnQte(Vector2 readValue)
        {
            if (_playerData.IsDead || _playerData.IsCarrying) return;
            
            // Debug.Log("Pad : " + readValue);
            if (_playerData.IsInteracting && readValue != Vector2.zero)
            {
                (int advancement, bool isCorrect, bool isFinished) tuple = _playerData.ActualPadQte.CheckQte(readValue);
                _playerData.PlayerWorldSpaceDisplayInteractions.DisplayQteAdvancement(tuple.advancement, tuple.isCorrect);
                
                // Events
                Events.Collecting();

                if (tuple.isFinished)
                {
                    QuitQte();
                }
            }
        }
        
        public void OnSwitchWeapon(float readValue)
        {
            if (_playerData.IsDead || _playerData.IsCarrying) return;
            
            // Debug.Log("ButtonNorth : " + readValue);
            if (_playerData.IsInteracting)
            {
                QuitQte();
            }
            
            _playerTwoBonesIkHandler.DisableTwoBonesIk();
            
            if (readValue > 0)
            {
                if (_playerData.EquippedWeapon.WeaponData._weaponTypesEnum == WeaponTypesEnum.Revolver)
                {
                    _playerData.EquippedWeapon.gameObject.SetActive(false);
                    _playerData.EquippedWeapon = _playerData.RifleWeapon;
                }
                else
                {
                    _playerData.EquippedWeapon.gameObject.SetActive(false);
                    _playerData.EquippedWeapon = _playerData.RevolverWeapon;
                }
                
                Invoke(nameof(EnableTwoBonesIk), 0.5f);
                
                // Animator
                _animator.SetTrigger("SwitchWeapon");
            }
        }

        public void OnInteract(float readValue)
        {
            if (_playerData.IsDead) return;
            
            // Debug.Log("ButtonSouth : " + readValue);
            
            if (readValue > 0)
            {
                _playerData.PressesButtonSouth = true;

                if (_playerData.IsCarrying)
                {
                    List<GameObject> towersGameObjects = _playerData.InteractableTriggerCollider.GetGameObjectsWithTag("Tower");

                    if (towersGameObjects.Count > 0)
                    {
                        _playerData.IsCarrying = false;
                        towersGameObjects[0].GetComponent<TowerHandler>().ReceiveShield();
                        _playerData.LastInteractableShield.Disappears();
                        return;
                    }
                }

                GameObject nearestInteractable = _playerData.InteractableTriggerCollider.GetNearestObject();
                
                if (nearestInteractable && !_playerData.IsInteracting)
                {
                    if (nearestInteractable.CompareTag("Resource"))
                    {
                        Debug.Log("Interact with a Resource");
                        if (_movement != Vector2.zero) return;
                        
                        _playerData.LastInteractableResource = nearestInteractable.GetComponent<ResourceHandler>();
                        if (_playerData.LastInteractableResource.CurrentCapacity > 0)
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
                        
                        _playerData.LastInteractableTower = nearestInteractable.GetComponent<TowerHandler>();
                        _playerData.Inventory = _playerData.LastInteractableTower.ReceiveResources(_playerData.Inventory);
                    }
                    if (nearestInteractable.CompareTag("Reload"))
                    {
                        Debug.Log("Interact with a Reload");
                        if (_movement != Vector2.zero) return;
                        
                        _playerData.LastInteractableReloadHealing = nearestInteractable.GetComponent<ReloadHealingHandler>();
                        _playerData.WantToReload = true;
                    }
                    if (nearestInteractable.CompareTag("Heal"))
                    {
                        Debug.Log("Interact with a Heal");
                        if (_movement != Vector2.zero) return;
                        
                        _playerData.LastInteractableReloadHealing = nearestInteractable.GetComponent<ReloadHealingHandler>();
                        _playerData.WantToHeal = true;
                    }
                    if (nearestInteractable.CompareTag("PowerPlant"))
                    {
                        Debug.Log("Interact with a PowerPlant");
                        if (_movement != Vector2.zero) return;
                        
                        _playerData.LastInteractablePowerPlant = nearestInteractable.GetComponent<PowerPlantHandler>();
                        
                        if (!_playerData.LastInteractablePowerPlant.IsRepaired)
                        {
                            StartCoroutine(TmeBeforeCollecting("PowerPlant"));
                        }
                    }
                    if (nearestInteractable.CompareTag("Shield"))
                    {
                        Debug.Log("Interact with a Shield");

                        _playerData.LastInteractableShield = nearestInteractable.GetComponent<ShieldHandler>();

                        if (!_playerData.IsCarrying)
                        {
                            _playerData.IsCarrying = true;
                            _playerData.LastInteractableShield.Carrie(_playerData.CarrieShieldTransform);
                            _playerData.PlayerWorldSpaceDisplayInteractions.HideInteractionsButton();
                        }
                        else
                        {
                            _playerData.IsCarrying = false;
                            _playerData.LastInteractableShield.Drop();
                        }
                    }
                    if (nearestInteractable.CompareTag("Lobby"))
                    {
                        Debug.Log("Interact with a Lobby");
                        // if (GameManager.Instance.Players.Count < 4) return;
                        
                        SceneManager.LoadScene(2);
                    }
                }
            }
            else
            {
                _playerData.PressesButtonSouth = false;
                _playerData.WantToReload = false;
                _playerData.WantToHeal = false;
                
                // Display
                _playerData.PlayerWorldSpaceDisplayInteractions.ResetInteractionButtonFill();
                
                // Events
                Events.Reloading(false);
                Events.Healing(false);
            }
        }

        public void OnPush(float readValue)
        {
            if (_playerData.IsDead || _playerData.IsCarrying) return;
            
            // Debug.Log("ButtonWest : " + readValue);
            if (_playerData.IsInteracting)
            {
                QuitQte();
            }

            if (readValue > 0)
            {
                _playerInputHandler.InputAreEnable = false;
                
                _playerTwoBonesIkHandler.DisableTwoBonesIk();
                _playerData.EquippedWeapon.gameObject.SetActive(false);
                
                StartCoroutine(SetEnableInputs(true, _playerData.PlayerBaseStats.PushDuration));
                
                // Animator
                _animator.SetTrigger("Push");
                
                // Events
                Events.Pushing();
            }
        }

        public void OnShoot(float readValue)
        {
            if (_playerData.IsDead || _playerData.IsCarrying) return;
            
            // Debug.Log("R2 : " + readValue);
            if (_playerData.IsInteracting)
            {
                QuitQte();
            }

            if (Mathf.Approximately(readValue, 1))
            {
                _playerData.IsShooting = true;
            }
            else
            {
                _playerData.IsShooting = false;
            }
            
            _playerData.EquippedWeapon.Shoot(readValue);
        }

        public void OnStart(float readValue)
        {
            if (readValue > 0)
            {
                Debug.Log("Start");
            }
        }

        public (float, float, WeaponHandler, Dictionary<ResourcesEnum, int>, int, int, int) GetCanvasHudData()
        {
            return (_playerData.PlayerBaseStats.MaxHealth, _playerData.CurrentHealth, _playerData.EquippedWeapon, _playerData.Inventory, 
                _playerData.PlayerBaseStats.MaxStone, _playerData.PlayerBaseStats.MaxMetal, _playerData.PlayerBaseStats.MaxCircuit);
        }

        public (float, float, float, float) GetCanvasWorldSpaceData()
        {
            return (_playerData.PlayerBaseStats.MaxHealth, _playerData.CurrentHealth, _playerData.EquippedWeapon.WeaponData.OverheatingLimit,
                _playerData.EquippedWeapon.CurrentOverheating);
        }
        
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "LobbyTest")
            {
                transform.position = GameManager.Instance.SpawnPointsLobby[_playerData.PlayerId];
                
            }
            else if (arg0.name == "InGame_Test")
            {
                transform.position = GameManager.Instance.SpawnPointsInGame[_playerData.PlayerId];
            }
            
            _playerInputHandler.InputAreEnable = true;
            _playerData.CanvasHandler.OnSceneLoaded();
            _playerData.PlayerWorldSpaceDisplayInteractions.HideInteractionsButton();
            
            _playerData.Inventory[ResourcesEnum.Stone] = 200;
            _playerData.Inventory[ResourcesEnum.Metal] = 200;
            _playerData.Inventory[ResourcesEnum.ElectricalCircuit] = 200;
        }

        private IEnumerator TmeBeforeCollecting(string interactableName)
        {
            float time = 0f;
            while (_playerData.PressesButtonSouth)
            {
                time += 0.01f;
                _playerData.PlayerWorldSpaceDisplayInteractions.UpdateInteractionButtonFill(time, _playerData.PlayerBaseStats.TimeBeforeCollecting);

                if (time >= _playerData.PlayerBaseStats.TimeBeforeCollecting)
                {
                    _playerData.IsInteracting = true;
                    _playerData.ActualInteractableName = interactableName;
                    
                    _playerTwoBonesIkHandler.DisableTwoBonesIk();
                    _playerData.EquippedWeapon.gameObject.SetActive(false);

                    if (interactableName == "Resource")
                    {
                        (ResourcesEnum, int) resource = _playerData.LastInteractableResource.ResourcesICanCollect();
                        int size = 0;
                        
                        if (resource.Item1 == ResourcesEnum.Stone)
                        {
                            size = Mathf.Min(_playerData.PlayerBaseStats.MaxStone - _playerData.Inventory[resource.Item1], resource.Item2);
                        }
                        if (resource.Item1 == ResourcesEnum.Metal)
                        {
                            size = Mathf.Min(_playerData.PlayerBaseStats.MaxMetal - _playerData.Inventory[resource.Item1], resource.Item2);
                        }
                        if (resource.Item1 == ResourcesEnum.ElectricalCircuit)
                        {
                            size = Mathf.Min(_playerData.PlayerBaseStats.MaxCircuit - _playerData.Inventory[resource.Item1], resource.Item2);
                        }
                        
                        _playerData.ActualPadQte = new PadQte(size);
                    }
                    if (interactableName == "PowerPlant")
                    {
                        _playerData.ActualPadQte = new PadQte(_playerData.LastInteractablePowerPlant.QteSize);
                    }
                    
                    // Display
                    _playerData.PlayerWorldSpaceDisplayInteractions.HideInteractionsButton();
                    _playerData.PlayerWorldSpaceDisplayInteractions.DisplayQteButton(_playerData.ActualPadQte.Qte);
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        private void QuitQte()
        {
            _playerData.IsInteracting = false;
            
            _playerTwoBonesIkHandler.EnableTwoBonesIk(_playerData.EquippedWeapon.WeaponData);
            _playerData.EquippedWeapon.gameObject.SetActive(true);
            
            if (_playerData.ActualInteractableName == "Resource")
            {
                (ResourcesEnum resource, int value) tupleResource = _playerData.LastInteractableResource.GetResources(_playerData.ActualPadQte.Score);
                _playerData.Inventory[tupleResource.resource] += tupleResource.value;
            }
            if (_playerData.ActualInteractableName == "PowerPlant")
            {
                if (_playerData.ActualPadQte.Score == _playerData.LastInteractablePowerPlant.QteSize)
                {
                    _playerData.LastInteractablePowerPlant.Repair();
                }
            }
            
            // Display
            _playerData.PlayerWorldSpaceDisplayInteractions.HideQteButton();
            _playerData.PlayerWorldSpaceDisplayInteractions.DisplayInteractionsButton();
        }

        private IEnumerator SetEnableInputs(bool enable, float duration)
        {
            yield return new WaitForSeconds(duration);
            _playerInputHandler.InputAreEnable = enable;
            
            _playerTwoBonesIkHandler.EnableTwoBonesIk(_playerData.EquippedWeapon.WeaponData);
            _playerData.EquippedWeapon.gameObject.SetActive(true);
        }

        public void TakeDamage(float damage)
        {
            if (_playerData.IsDead) return;
            
            _playerData.CurrentHealth -= damage;

            if (_playerData.CurrentHealth <= 0)
            {
                Die();
            }
            
            // Animator
            _animator.SetTrigger("TakeDamage");
            
            // Event
            Events.TakingDamage();
        }

        public void ApplyPush()
        {
            List<GameObject> enemiesGameObjects = new List<GameObject>();
            enemiesGameObjects = _playerData.RepelTriggerCollider.GetGameObjectsWithTag("Enemy");

            Debug.Log("Enemy pushed : " + enemiesGameObjects.Count);
            foreach (GameObject enemy in enemiesGameObjects)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<EnemyAIHandler>().IsPushed(direction, _playerData.PlayerBaseStats.PushForce, _playerData.PlayerBaseStats.PushDuration);
            } 
        }

        private void EnableTwoBonesIk()
        {
            _playerTwoBonesIkHandler.EnableTwoBonesIk(_playerData.EquippedWeapon.WeaponData);
            _playerData.EquippedWeapon.gameObject.SetActive(true);
        }

        public void SetUp(int id, PlayerBaseData playerBaseData)
        {
            _playerData.PlayerId = id;
            _playerData.CircleImage.color = playerBaseData.Color;
        }

        private void Die()
        {
            Debug.Log("Die");
            _playerTwoBonesIkHandler.DisableTwoBonesIk();
            _playerData.EquippedWeapon.gameObject.SetActive(false);
            
            GameManager.Instance.APlayerDie(gameObject);
            Destroy(gameObject, 2f);
            
            _playerData.IsDead = true;
            
            // Animation
            _animator.SetTrigger("IsDead");
        }
    }
}