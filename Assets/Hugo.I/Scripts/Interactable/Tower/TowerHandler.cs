using System.Collections.Generic;
using Hugo.I.Scripts.Game;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerHandler : MonoBehaviour, IHaveHealth
    {
        [Header("Settings")]
        [SerializeField] private TowerManager _towerManager;
        [SerializeField] private TowerLevelData _towerLevelData;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _currentEnergy;
        [SerializeField] private int _currentStone;
        [SerializeField] private int _currentMetal;
        [SerializeField] private int _currentElectricalCircuit;
        [SerializeField] private bool _isRestoringCapacity;
        [SerializeField] private bool _isGivingCapacity;

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, _towerLevelData.MaxHealth);
        }
        
        public float CurrentEnergy
        {
            get => _currentEnergy;
            set
            {
                _currentEnergy = Mathf.Clamp(value, 0, _towerLevelData.MaxEnergy);
                _isRestoringCapacity = _currentEnergy < _towerLevelData.MaxEnergy;
            }
        }

        private void Awake()
        {
            CurrentEnergy = 0;
            CurrentHealth = _towerLevelData.MaxHealth;
        }

        private void Update()
        {
            if (_isRestoringCapacity && GameManager.IsPowerPlantRepairs)
            {
                CurrentEnergy += _towerLevelData.EnergyRestoreRate * Time.deltaTime;
            }
        }

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
            
            CheckUpgrade();
            
            return newPlayerInventory;
        }

        public void GiveEnergy()
        {
            CurrentEnergy -= _towerLevelData.EnergyDecreaseRate * Time.deltaTime;
        }

        public (TowerLevelData, int, int, int) GetTowerData()
        {
            return (_towerLevelData, _currentStone, _currentMetal, _currentElectricalCircuit) ;
        }

        private void CheckUpgrade()
        {
            if (_currentStone >= _towerLevelData.StoneToLevelUp && _currentMetal >= _towerLevelData.MetalToLevelUp
                && _currentElectricalCircuit >= _towerLevelData.ElectricalCircuitToLevelUp)
            {
                _towerManager.UpgradeTower(gameObject);
            }
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
        }
    }
}