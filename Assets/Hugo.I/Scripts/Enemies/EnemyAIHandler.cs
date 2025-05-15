using Hugo.I.Scripts.Enemies.States;
using Hugo.I.Scripts.Utils;
using Unity.Cinemachine;
using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyAIHandler : EnemyData, IHaveHealth
    {
        private void Update()
        {
            if (TargetGameObject == TowerHandler.gameObject)
            {
                if (TargetGameObject && !HasReachDestination)
                {
                    CurrentState = new GoToTarget();
                }
                else if (TargetGameObject && HasReachDestination)
                {
                    CurrentState = new Attack();
                }
            }
            else if (TargetGameObject == PlayerController.gameObject)
            {
                if (TargetGameObject && !HaveRangeToAttackTarget)
                {
                    CurrentState = new GoToTarget();
                }
                else if (TargetGameObject && HaveRangeToAttackTarget)
                {
                    CurrentState = new Attack();
                }
            }

            if (IsDead)
            {
                CurrentState = new Dead();
            }
            
            CurrentState.Execute(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.DrawWireSphere(transform.position, RangeDontSwitchTarget);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
        }
    }
}
