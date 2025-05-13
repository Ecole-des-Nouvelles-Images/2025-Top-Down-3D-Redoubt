using Hugo.I.Scripts.Utils;

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
                enemy.IsAttacking = true;
                enemy.StartCoroutine(enemy.CoroutineIsAttacking());
            }
        }
    }
}