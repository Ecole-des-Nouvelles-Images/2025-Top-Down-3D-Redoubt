namespace Hugo.I.Scripts.Enemies.States
{
    public class Attack : State
    {
        public override void Execute(EnemyData enemy)
        {
            if (!enemy.IsAttacking)
            {
                enemy.NavMeshAgent.ResetPath();
                enemy.IsAttacking = true;
                enemy.StartCoroutine(enemy.CoroutineIsAttacking());
                
                // Animation
                enemy.Animator.SetTrigger("Attack");
            }
        }
    }
}