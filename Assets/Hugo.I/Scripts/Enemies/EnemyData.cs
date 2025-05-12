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
        public float CurrentHealth;
        
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
        public bool IsAttacking;
        public bool IsDead => CurrentHealth <= 0;

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

        [Header("Internal Components")]
        public NavMeshAgent NavMeshAgent;
        public Rigidbody Rigidbody;

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Rigidbody = GetComponent<Rigidbody>();
            
            NavMeshAgent.speed = WalkSpeed;
            NavMeshAgent.angularSpeed = AngularSpeed;
            NavMeshAgent.acceleration = Acceleration;
            
            CurrentHealth = MaxHealth;
            
            TowerHandler = GameManager.ActualTowerGameObject.GetComponent<TowerHandler>();
        }

        public IEnumerator CoroutineIsAttacking()
        {
            yield return new WaitForSeconds(AttackSpeed);
            IsAttacking = false;
        }
    }
}