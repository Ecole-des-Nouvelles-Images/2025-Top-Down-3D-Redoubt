using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerLevelsManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TowerLevelsEnum _actualTowerLevels;
        [SerializeField] private List<GameObject> _towers;

        public void UpgradeTower(GameObject currentTower)
        {
            int index = _towers.IndexOf(currentTower);
            Debug.Log(index);
            _towers[index].gameObject.SetActive(false);
            _towers[index + 1].gameObject.SetActive(true);
        }
    }
}
