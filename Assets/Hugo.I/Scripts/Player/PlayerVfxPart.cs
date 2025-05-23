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
        [SerializeField] private ParticleSystem _vfxCollecting;
        [SerializeField] private ParticleSystem _vfxTakingDamage;
        [SerializeField] private ParticleSystem _vfxPushing;

        private void OnEnable()
        {
            if (!_playerController) return;

            _playerController.Events.OnMove += Move;
            _playerController.Events.OnHealing += Healing;
            _playerController.Events.OnReloading += Reloading;
            _playerController.Events.OnCollecting += Collecting;
            _playerController.Events.OnTakingDamage += TakingDamage;
            _playerController.Events.OnPushing += Pushing;
        }

        private void OnDisable()
        {
            if (!_playerController) return;
            
            _playerController.Events.OnMove -= Move;
            _playerController.Events.OnHealing -= Healing;
            _playerController.Events.OnReloading -= Reloading;
            _playerController.Events.OnCollecting -= Collecting;
            _playerController.Events.OnTakingDamage -= TakingDamage;
            _playerController.Events.OnPushing -= Pushing;
        }
        
        private void Move(float velocity)
        {
            if (!_vfxMove) return;
            
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
            if (!_vfxHealing) return;
            
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
            if (!_vfxReloading) return;
            
            if (isReloading && !_vfxReloading.isPlaying)
            {
                _vfxReloading.Play();
            }
            else if (!isReloading && _vfxReloading.isPlaying)
            {
                _vfxReloading.Stop();
            }
        }
        
        private void Collecting()
        {
            if (!_vfxCollecting) return;
            _vfxCollecting.Play();
        }
        
        private void TakingDamage()
        {
            if (!_vfxTakingDamage) return;
            _vfxTakingDamage.Play();
        }
        
        private void Pushing()
        {
            if (!_vfxPushing) return;
            _vfxPushing.Play();
        }
    }
}
