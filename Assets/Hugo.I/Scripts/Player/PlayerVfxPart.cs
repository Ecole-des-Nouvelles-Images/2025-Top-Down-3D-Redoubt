using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class PlayerVfxPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        
        [Header("Vfx Effects")]
        [SerializeField] private ParticleSystem _vfxMove;
        [SerializeField] private ParticleSystem _vfxHealing;
        [SerializeField] private ParticleSystem _vfxReloading;

        private void OnEnable()
        {
            if (!_playerController) return;

            _playerController.Events.OnMove += Move;
            _playerController.Events.OnHealing += Healing;
            _playerController.Events.OnReloading += Reloading;
        }

        private void OnDisable()
        {
            if (!_playerController) return;
            
            _playerController.Events.OnMove -= Move;
            _playerController.Events.OnHealing -= Healing;
            _playerController.Events.OnReloading -= Reloading;
        }
        
        private void Move(float velocity)
        {
            bool isMoving = velocity > 0.1f;

            if (isMoving && !_vfxMove.isPlaying)
            {
                _vfxMove.Play();
            }
            else if (!isMoving && _vfxMove.isPlaying)
            {
                _vfxMove.Stop();
            }
        }
        
        private void Healing(bool isHealing)
        {
            if (isHealing && !_vfxHealing.isPlaying)
            {
                _vfxHealing.Play();
            }
            else if (!isHealing && _vfxMove.isPlaying)
            {
                _vfxHealing.Stop();
            }
        }
        
        private void Reloading(bool isReloading)
        {
            if (isReloading && !_vfxReloading.isPlaying)
            {
                _vfxReloading.Play();
            }
            else if (!isReloading && _vfxReloading.isPlaying)
            {
                _vfxReloading.Stop();
            }
        }
    }
}
