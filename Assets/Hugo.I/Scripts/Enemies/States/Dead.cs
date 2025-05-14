namespace Hugo.I.Scripts.Enemies.States
{
    public class Dead : State
    {
        public override void Execute(EnemyData enemy)
        {
            if (!enemy.IsDying)
            {
                enemy.IsDying = true;
                enemy.Collider.enabled = false;
                enemy.NavMeshAgent.ResetPath();
                enemy.NavMeshAgent.isStopped = true;
                enemy.EnemySpawnerManager.CurrentCredit--;
                enemy.Die();
            }
        }
    }
}
