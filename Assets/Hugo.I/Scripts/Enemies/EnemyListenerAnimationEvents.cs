using Hugo.I.Scripts.Sounds;
using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyListenerAnimationEvents : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyAIHandler _enemyAIHandler;
        
        public void FootStep()
        {
            _enemyAIHandler.Events.FootStep();
            
            // Sound
            SoundManager.Instance.PlaySound(gameObject, SoundManager.Instance.EnemyFootStepSounds);
        }
        
        public void HitAttack()
        {
            _enemyAIHandler.Events.HitAttack();
            _enemyAIHandler.DoneDamage();
            
            // Sound
            SoundManager.Instance.PlaySound(gameObject, SoundManager.Instance.EnemyAttackImpactSounds);
        }
    }
}