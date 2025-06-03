using Hugo.I.Scripts.Sounds;
using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class ReloadHealingHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _isHealingZone;
        [SerializeField] private TowerManager _towerManager;

        public bool UseEnergy()
        {
            TowerHandler towerHandler = _towerManager.ActiveTower.GetComponent<TowerHandler>();
            towerHandler.GiveEnergy();

            if (!GetComponent<AudioSource>())
            {
                if (_isHealingZone)
                {
                    SoundManager.Instance.PlaySound(gameObject, SoundManager.Instance.HealingSounds);
                }
                else
                {
                    SoundManager.Instance.PlaySound(gameObject, SoundManager.Instance.ReloadingSounds);
                }
            }
            
            return towerHandler.CurrentEnergy > 0;
        }
    }
}