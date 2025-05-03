using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TowerLevelData _towerLevelData;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _currentCapacity;
        [SerializeField] private int _currentStone;
        [SerializeField] private int _currentMetal;
        [SerializeField] private int _currentElectricalCircuit;

        public Dictionary<ResourcesEnum, int> ReceiveResources(Dictionary<ResourcesEnum, int> resourcesToGive)
        {
            Dictionary<ResourcesEnum, int> newPlayerInventory = new Dictionary<ResourcesEnum, int>();
            
            if (resourcesToGive[ResourcesEnum.Stone] + _currentStone <= _towerLevelData.StoneToLevelUp)
            {
                _currentStone += resourcesToGive[ResourcesEnum.Stone];
            }
            else
            {
                newPlayerInventory[ResourcesEnum.Stone] = _currentStone + resourcesToGive[ResourcesEnum.Stone] - _towerLevelData.StoneToLevelUp;
                _currentStone = _towerLevelData.StoneToLevelUp;
            }
            
            if (resourcesToGive[ResourcesEnum.Metal] + _currentMetal <= _towerLevelData.MetalToLevelUp)
            {
                _currentMetal += resourcesToGive[ResourcesEnum.Metal];
            }
            else
            {
                newPlayerInventory[ResourcesEnum.Metal] = _currentMetal + resourcesToGive[ResourcesEnum.Metal] - _towerLevelData.MetalToLevelUp;
                _currentMetal = _towerLevelData.MetalToLevelUp;
            }
            
            if (resourcesToGive[ResourcesEnum.ElectricalCircuit] + _currentElectricalCircuit <= _towerLevelData.ElectricalCircuitToLevelUp)
            {
                _currentElectricalCircuit += resourcesToGive[ResourcesEnum.ElectricalCircuit];
            }
            else
            {
                newPlayerInventory[ResourcesEnum.ElectricalCircuit] = _currentElectricalCircuit + resourcesToGive[ResourcesEnum.ElectricalCircuit] - _towerLevelData.ElectricalCircuitToLevelUp;
                _currentElectricalCircuit = _towerLevelData.ElectricalCircuitToLevelUp;
            }
            
            return newPlayerInventory;
        }
    }
}
