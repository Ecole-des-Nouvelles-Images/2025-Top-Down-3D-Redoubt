using System;
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
                Debug.Log("Move");
                _vfxMove.Play();
            }
            else if (!isMoving && _vfxMove.isPlaying)
            {
                Debug.Log("Don't Move");
                _vfxMove.Stop();
            }
        }
        
        private void Healing()
        {
            Debug.Log("Heal");
            _vfxHealing.Play();
        }
        
        private void Reloading()
        {
            Debug.Log("Reload");
            _vfxReloading.Play();
        }
    }
}
