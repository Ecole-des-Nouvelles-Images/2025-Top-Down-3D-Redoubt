using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerLevelsManager : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private TowerLevelsEnum _actualTowerLevels;
        [SerializeField] private TowerLevelData _towerLevelT0;
        [SerializeField] private TowerLevelData _towerLevelT1;
        [SerializeField] private TowerLevelData _towerLevelT2;
        [SerializeField] private TowerLevelData _towerLevelT3;
        
        // Inventory
        private Dictionary<ResourcesEnum, int> _storage = new Dictionary<ResourcesEnum, int>()
        {
            { ResourcesEnum.Stone, 0 },
            { ResourcesEnum.Metal, 0 },
            { ResourcesEnum.ElectricalCircuit, 0 }
        };

        private int _currentStone;
        private int _currentMetal;
        private int _currentElectronicalCircuit;

        public TowerLevelsEnum ActualTowerLevels
        {
            get => _actualTowerLevels;
            set
            {
                if (_actualTowerLevels == value) return;
                
                _actualTowerLevels = value;
                // SetTower(value);
            }
        }

        private void Awake()
        {
            UpgradeTower(_actualTowerLevels);
        }

        // Datas
        private TowerLevelData _actualTowerLevelData;

        [ContextMenu("Update Tower")]
        public void UpdateTower()
        {
            ActualTowerLevels = _actualTowerLevels;
        }

        // private void SetTower(TowerLevelsEnum towerLevel)
        // {
        //     if (towerLevel == TowerLevelsEnum.T0)
        //     {
        //         _towerLevelT0.gameObject.SetActive(true);
        //         _towerLevelT1.gameObject.SetActive(false);
        //         _towerLevelT2.gameObject.SetActive(false);
        //         _towerLevelT3.gameObject.SetActive(false);
        //     }
        //     if (towerLevel == TowerLevelsEnum.T1)
        //     {
        //         _towerLevelT0.gameObject.SetActive(false);
        //         _towerLevelT1.gameObject.SetActive(true);
        //         _towerLevelT2.gameObject.SetActive(false);
        //         _towerLevelT3.gameObject.SetActive(false);
        //     }
        //     if (towerLevel == TowerLevelsEnum.T2)
        //     {
        //         _towerLevelT0.gameObject.SetActive(false);
        //         _towerLevelT1.gameObject.SetActive(false);
        //         _towerLevelT2.gameObject.SetActive(true);
        //         _towerLevelT3.gameObject.SetActive(false);
        //     }
        //     if (towerLevel == TowerLevelsEnum.T3)
        //     {
        //         _towerLevelT0.gameObject.SetActive(false);
        //         _towerLevelT1.gameObject.SetActive(false);
        //         _towerLevelT2.gameObject.SetActive(false);
        //         _towerLevelT3.gameObject.SetActive(true);
        //     }
        //     
        //     ActualTowerLevels = towerLevel;
        // }

        private void UpgradeTower(TowerLevelsEnum towerLevel)
        {
            Debug.Log($"Tower Level: {towerLevel}");
        }

        public void TakeResources(Dictionary<ResourcesEnum, int> resources)
        {
            foreach (KeyValuePair<ResourcesEnum, int> resource in resources)
            {
                _storage[resource.Key] += resource.Value;
            }
        }

        public void OnEnterZone()
        {
            throw new System.NotImplementedException();
        }

        public void OnExitZone()
        {
            throw new System.NotImplementedException();
        }
    }
}
