using Hugo.I.Scripts.Enemies.States;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyAIHandler : EnemyData
    {
        private void Update()
        {
            if (!CanAttack)
            {
                CurrentState = new GoToTarget();
            }
            else
            {
                CurrentState = new Attack();
            }

            if (IsDead)
            {
                CurrentState = new Dead();
            }
            
            CurrentState.Execute(this);
        }
    }
}
