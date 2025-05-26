using System.Collections;
using Hugo.I.Scripts.Enemies.States;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Hugo.I.Scripts.Enemies
{
    public abstract class EnemyData : MonoBehaviour
    {
        [Header("   Settings")]
        
        [Header("Currents")]
        public State CurrentState;
        private float _currentHealth;
        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        }
        
        [Header("Datas")]
        public float MaxHealth;
        public float Damage;
        public float AttackSpeed;
        public float AttackRange;
        public float RangeDontSwitchTarget;
        public float WalkSpeed;
        public float AngularSpeed;
        public float Acceleration;

        [Header("States")]
        public bool HaveRangeToAttackTarget => TargetGameObject && Vector3.Distance(transform.position, TargetGameObject.transform.position) <= AttackRange;
        public bool HasReachDestination;
        public bool IsAttacking;
        public bool IsPush;
        public bool IsDead => CurrentHealth <= 0;
        public bool IsDying;

        [Header("Target")]
        public TowerHandler TowerHandler;
        public PlayerController PlayerController;
        public GameObject TargetGameObject
        {
            get
            {
                if (Vector3.Distance(transform.position, TowerHandler.transform.position) >= RangeDontSwitchTarget)
                {
                    if (PlayerController)
                    {
                        return PlayerController.gameObject;
                    }
                    
                    if (TowerHandler)
                    {
                        return TowerHandler.gameObject;
                    }
                }
                else
                {
                    return TowerHandler.gameObject;
                }

                return null;
            }
        }

        [Header("Events")]
        public EnemyEvents Events { get; private set; } = new EnemyEvents();

        [Header("Internal Components")]
        public NavMeshAgent NavMeshAgent;
        public Rigidbody Rigidbody;
        public Collider Collider;
        public Animator Animator;

        [Header("External Components")]
        public EnemySpawnerManager EnemySpawnerManager;

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
            
            NavMeshAgent.speed = WalkSpeed;
            NavMeshAgent.angularSpeed = AngularSpeed;
            NavMeshAgent.acceleration = Acceleration;
            
            CurrentHealth = MaxHealth;
            
            TowerHandler = GameManager.Instance.ActualTowerGameObject.GetComponent<TowerHandler>();
        }

        private void Start()
        {
            EnemySpawnerManager = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawnerManager>();
        }

        public IEnumerator CoroutineIsAttacking()
        {
            yield return new WaitForSeconds(AttackSpeed);
            IsAttacking = false;
        }

        public void IsPushed(Vector3 direction, float force, float duration)
        {
            Debug.Log("Is Pushed");
            
            StartCoroutine(ApplyPush(direction, force, duration));
            HasReachDestination = false;
            
            // Animation
            Animator.SetTrigger("IsPush");
        }

        public void Die()
        {
            Destroy(gameObject, 3f);
        }
        
        private IEnumerator ApplyPush(Vector3 direction, float force, float duration)
        {
            IsPush = true;
            
            NavMeshAgent.isStopped = true;
            NavMeshAgent.enabled = false;

            Rigidbody.isKinematic = false;
            Rigidbody.freezeRotation = true;
            Rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
            Debug.Log("Apply Push. Direction : " + direction);

            yield return new WaitForSeconds(duration);

            IsPush = false;
            
            Rigidbody.isKinematic = true;
            Rigidbody.freezeRotation = false;
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            NavMeshAgent.enabled = true;
            NavMeshAgent.isStopped = false;
        }
    }
}