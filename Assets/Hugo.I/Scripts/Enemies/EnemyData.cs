using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public abstract class EnemyData : MonoBehaviour
    {
        [Header("   Settings")]
        
        [Header("Currents")]
        public float CurrentHealth;
        
        [Header("Datas")]
        public float MaxHealth;
        public float IncreaseHealthRate;
        public int Damage;
        public int Range;
        public float WalkSpeed;

        [Header("Boll")]
        public bool AttackTower;
        public bool AttackPowerPlant;
        public bool AttackPlayer;
        public bool IsDead;
    }
}