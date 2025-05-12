using UnityEngine;

namespace Hugo.I.Scripts.Enemies.States
{
    public class GoToTarget : State
    {
        public override void Execute(EnemyData enemy)
        {
            // Debug.Log("Go to Target : " + enemy.TargetGameObject.name);

            enemy.NavMeshAgent.SetDestination(enemy.TargetGameObject.transform.position);
        }
    }
}