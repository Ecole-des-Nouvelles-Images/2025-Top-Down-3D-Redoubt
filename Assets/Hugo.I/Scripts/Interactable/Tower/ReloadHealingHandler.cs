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
            towerHandler.GiveEnergy();
            
            return towerHandler.CurrentEnergy > 0;
        }
    }
}