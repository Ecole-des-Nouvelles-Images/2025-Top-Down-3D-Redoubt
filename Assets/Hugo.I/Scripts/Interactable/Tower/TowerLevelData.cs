using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    [CreateAssetMenu(fileName = "TowerLevelData", menuName = "Scriptable Objects/TowerLevelData")]
    public class TowerLevelData : ScriptableObject
    {
        public Sprite Icon;
        public TowerLevelsEnum _towerLevelsEnum;
        public float Health;
        public float Capacity;
        public int StoneToLevelUp;
        public int MetalToLevelUp;
        public int ElectricalCircuitToLevelUp;
    }
}
