using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyVfxPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyAIHandler _enemyAIHandler;
        
        [Header("Vfx Effect")]
        [SerializeField] private ParticleSystem _vfxFootStep;
        [SerializeField] private ParticleSystem _vfxHit;
        [SerializeField] private ParticleSystem _vfxTakeDamage;

        private void OnEnable()
        {
            if (!_enemyAIHandler) return;

            _enemyAIHandler.Events.OnTakeDamage += TakeDamage;
            _enemyAIHandler.Events.OnFootStep += FootStep;
            _enemyAIHandler.Events.OnHitAttack += HitAttack;
        }
        
        private void OnDisable()
        {
            if (!_enemyAIHandler) return;

            _enemyAIHandler.Events.OnTakeDamage -= TakeDamage;
            _enemyAIHandler.Events.OnFootStep -= FootStep;
            _enemyAIHandler.Events.OnHitAttack -= HitAttack;
        }

        private void TakeDamage()
        {
            if (!_vfxTakeDamage) return;
            _vfxTakeDamage.Play();
        }

        private void FootStep()
        {
            if (!_vfxFootStep) return;
            _vfxFootStep.Play();
        }

        private void HitAttack()
        {
            if (!_vfxHit) return;
            _vfxHit.Play();
        }
    }
}
