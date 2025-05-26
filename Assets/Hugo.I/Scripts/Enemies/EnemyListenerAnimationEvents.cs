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
        }
        
        public void HitAttack()
        {
            _enemyAIHandler.Events.HitAttack();
            _enemyAIHandler.DoneDamage();
        }
    }
}