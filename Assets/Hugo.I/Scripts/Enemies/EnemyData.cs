using Hugo.I.Scripts.Enemies.States;
using Hugo.I.Scripts.Interactable.PowerPlant;
using Hugo.I.Scripts.Interactable.Tower;
using Hugo.I.Scripts.Player;
using UnityEngine;

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
        public float IncreaseHealthRate;
        public float Damage;
        public float AttackRange;
        public float RangeDontSwitchTarget;
        public float WalkSpeed;

        [Header("Boll")]
        public bool TargetTower;
        public bool CanAttack => TargetGameObject && Vector3.Distance(transform.position, TargetGameObject.transform.position) <= AttackRange;
        public bool IsDead => CurrentHealth <= 0;

        public GameObject TargetGameObject
        {
            get => TargetGameObject;
            set
            {
                TargetGameObject = TowerHandler.gameObject;

                if (PowerPlantHandler.gameObject || PlayerController.gameObject 
                    && Vector3.Distance(transform.position, TowerHandler.gameObject.transform.position) > RangeDontSwitchTarget)
                {
                    if (PlayerController.gameObject)
                    {
                        TargetGameObject = PlayerController.gameObject;
                    }
                    else if (PowerPlantHandler.gameObject)
                    {
                        TargetGameObject = PowerPlantHandler.gameObject;
                    }
                    else
                    {
                        TargetGameObject = TowerHandler.gameObject;
                    }
                }
            }
        }

        public TowerHandler TowerHandler;
        public PowerPlantHandler PowerPlantHandler;
        public PlayerController PlayerController;
    }
}