using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerVfxPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerManager _towerManager;
        
        [Header("Vfx Effects")]
        [SerializeField] private ParticleSystem _vfxUpgrading;

        private void OnEnable()
        {
            if (!_towerManager) return;
            
            _towerManager.Events.OnUpgrading += Upgrading;
        }

        private void OnDisable()
        {
            if (!_towerManager) return;
            
            _towerManager.Events.OnUpgrading -= Upgrading;
        }
        
        private void Upgrading()
        {
            _vfxUpgrading.Play();
        }
    }
}