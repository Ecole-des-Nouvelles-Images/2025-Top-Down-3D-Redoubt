using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class PlayerVfxPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        
        [Header("Vfx Effects")]
        [SerializeField] private ParticleSystem _vfxWalk;
        [SerializeField] private ParticleSystem _vfxHealing;
        [SerializeField] private ParticleSystem _vfxReloading;
        [SerializeField] private ParticleSystem _vfxCollecting;
        [SerializeField] private ParticleSystem _vfxTakingDamage;
        [SerializeField] private ParticleSystem _vfxPushing;

        private void OnEnable()
        {
            if (!_playerController) return;

            _playerController.Events.OnHealing += Healing;
            _playerController.Events.OnReloading += Reloading;
            _playerController.Events.OnCollecting += Collecting;
            _playerController.Events.OnTakingDamage += TakingDamage;
            _playerController.Events.OnHitPush += HitPush;
            _playerController.Events.OnFootStep += FootStep;
        }

        private void OnDisable()
        {
            if (!_playerController) return;
            
            _playerController.Events.OnHealing -= Healing;
            _playerController.Events.OnReloading -= Reloading;
            _playerController.Events.OnCollecting -= Collecting;
            _playerController.Events.OnTakingDamage -= TakingDamage;
            _playerController.Events.OnHitPush -= HitPush;
            _playerController.Events.OnFootStep -= FootStep;
        }
        
        private void FootStep()
        {
            if (!_vfxWalk) return;
            _vfxWalk.Play();
        }
        
        private void Healing(bool isHealing)
        {
            if (!_vfxHealing) return;
            
            if (isHealing && !_vfxHealing.isPlaying)
            {
                _vfxHealing.Play();
            }
            else if (!isHealing && _vfxWalk.isPlaying)
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
        
        private void HitPush()
        {
            if (!_vfxPushing) return;
            _vfxPushing.Play();
        }
    }
}
