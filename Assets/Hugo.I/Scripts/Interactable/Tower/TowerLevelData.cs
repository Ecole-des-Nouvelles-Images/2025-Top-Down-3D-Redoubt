using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerLevelData : MonoBehaviour
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
