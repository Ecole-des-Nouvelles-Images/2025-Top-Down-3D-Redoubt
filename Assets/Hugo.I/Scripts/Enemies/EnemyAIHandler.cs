using Hugo.I.Scripts.Enemies.States;
using Hugo.I.Scripts.Utils;
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
            
            // Aninmator
            Animator.SetFloat("Move", NavMeshAgent.velocity.magnitude);
            Animator.SetBool("IsDead", IsDead);
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
            Animator.SetTrigger("TakeDamage");
        }
        
        public void DoneDamage()
        {
            Debug.Log("Done Damage");
            TargetGameObject.GetComponent<IHaveHealth>().TakeDamage(Damage);
        }
    }
}
