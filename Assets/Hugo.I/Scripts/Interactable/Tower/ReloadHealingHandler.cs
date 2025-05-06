using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class ReloadHealingHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TowerManager _towerManager;

        public bool UseEnergy()
        {
            TowerHandler towerHandler = _towerManager.ActiveTower.GetComponent<TowerHandler>();
            towerHandler.GiveCapacity();
            
            return towerHandler.CurrentCapacity > 0;
        }
    }
}