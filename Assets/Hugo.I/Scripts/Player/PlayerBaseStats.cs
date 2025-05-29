using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    [CreateAssetMenu(fileName = "PlayerBaseSats", menuName = "Scriptable Objects/PlayerBaseSats")]
    public class PlayerBaseStats : ScriptableObject
    {
        public float MaxHealth;
        public float IncreaseRateHealth;
        public float MoveSpeed;
        public float FactorAimingSpeed;
        public float FactorCarryingSpeed;
        public float PushForce;
        public float PushDuration;
        public int TimeBeforeCollecting;
        public int MaxStone;
        public int MaxMetal;
        public int MaxCircuit;
        public float GravityScale;
        
    }
}
