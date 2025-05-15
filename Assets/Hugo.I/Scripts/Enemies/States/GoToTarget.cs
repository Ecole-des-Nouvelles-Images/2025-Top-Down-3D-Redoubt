namespace Hugo.I.Scripts.Enemies.States
{
    public class GoToTarget : State
    {
        public override void Execute(EnemyData enemy)
        {
            if (enemy.NavMeshAgent.enabled)
            {
                enemy.NavMeshAgent.SetDestination(enemy.TargetGameObject.transform.position);

                if (enemy.NavMeshAgent.hasPath && enemy.TargetGameObject == enemy.TowerHandler.gameObject && enemy.NavMeshAgent.remainingDistance <= enemy.NavMeshAgent.stoppingDistance)
                {
                    enemy.HasReachDestination = true;
                }
            }
        }
    }
}