namespace Hugo.I.Scripts.Enemies.States
{
    public class GoToTarget : State
    {
        public override void Execute(EnemyData enemy)
        {
            enemy.NavMeshAgent.SetDestination(enemy.TargetGameObject.transform.position);
        }
    }
}