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
            Dictionary<ResourcesEnum, int> newPlayerInventory = new Dictionary<ResourcesEnum, int>()
            {
                { ResourcesEnum.Stone, 0 },
                { ResourcesEnum.Metal, 0 },
                { ResourcesEnum.ElectricalCircuit, 0 }
            };
            
            int stoneToGive = resourcesToGive[ResourcesEnum.Stone];
            int stoneLimit = _towerLevelData.StoneToLevelUp;
            newPlayerInventory[ResourcesEnum.Stone] = Mathf.Max(0, _currentStone + stoneToGive - stoneLimit);
            _currentStone = Mathf.Min(_currentStone + stoneToGive, stoneLimit);

            int metalToGive = resourcesToGive[ResourcesEnum.Metal];
            int metalLimit = _towerLevelData.MetalToLevelUp;
            newPlayerInventory[ResourcesEnum.Metal] = Mathf.Max(0, _currentMetal + metalToGive - metalLimit);
            _currentMetal = Mathf.Min(_currentMetal + metalToGive, metalLimit);

            int circuitToGive = resourcesToGive[ResourcesEnum.ElectricalCircuit];
            int circuitLimit = _towerLevelData.ElectricalCircuitToLevelUp;
            newPlayerInventory[ResourcesEnum.ElectricalCircuit] = Mathf.Max(0, _currentElectricalCircuit + circuitToGive - circuitLimit);
            _currentElectricalCircuit = Mathf.Min(_currentElectricalCircuit + circuitToGive, circuitLimit);
            
            return newPlayerInventory;
        }
    }
}
