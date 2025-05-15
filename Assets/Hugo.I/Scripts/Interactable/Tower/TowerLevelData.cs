using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    [CreateAssetMenu(fileName = "TowerLevelData", menuName = "Scriptable Objects/TowerLevelData")]
    public class TowerLevelData : ScriptableObject
    {
        public Sprite Icon;
        public TowerLevelsEnum TowerLevelsEnum;
        public float MaxHealth;
        public float HealthRestoreRate;
        public float MaxEnergy;
        public float EnergyRestoreRate;
        public float EnergyDecreaseRate;
        public int StoneToLevelUp;
        public int MetalToLevelUp;
        public int ElectricalCircuitToLevelUp;
    }
}