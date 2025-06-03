using Hugo.I.Scripts.Sounds;
using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class PlayerListenerAnimationEvents : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;

        public void FootStep()
        {
            _playerController.Events.FootStep();
            
            // Sound
            SoundManager.Instance.PlaySound(gameObject, SoundManager.Instance.PlayerFootStepSounds);
        }

        public void HitPush()
        {
            _playerController.Events.HitPush();
            
            _playerController.ApplyPush();
        }
    }
}
