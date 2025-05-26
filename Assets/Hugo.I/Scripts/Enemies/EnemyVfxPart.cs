using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyVfxPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyAIHandler _enemyAIHandler;
        
        [Header("Vfx Effect")]
        [SerializeField] private ParticleSystem _vfxFootStep;

        private void OnEnable()
        {
            if (!_enemyAIHandler) return;

            _enemyAIHandler.Events.OnFootStep += FootStep;
        }
        
        private void OnDisable()
        {
            if (!_enemyAIHandler) return;

            _enemyAIHandler.Events.OnFootStep -= FootStep;
        }

        private void FootStep()
        {
            if (!_vfxFootStep) return;
            _vfxFootStep.Play();
        }
    }
}
