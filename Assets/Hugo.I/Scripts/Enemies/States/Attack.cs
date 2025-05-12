using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Enemies.States
{
    public class Attack : State
    {
        public override void Execute(EnemyData enemy)
        {
            if (!enemy.IsAttacking)
            {
                enemy.NavMeshAgent.ResetPath();
                enemy.TargetGameObject.GetComponent<IHaveHealth>().TakeDamage(enemy.Damage);
                // Debug.Log("Attack : " + enemy.TargetGameObject.name);
                enemy.IsAttacking = true;
                enemy.StartCoroutine(enemy.CoroutineIsAttacking());
            }
        }
    }
}
