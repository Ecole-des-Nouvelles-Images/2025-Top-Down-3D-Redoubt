using System;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyEvents
    {
        public event Action OnFootStep;
        public event Action OnHitAttack;
        
        public void FootStep() => OnFootStep?.Invoke();
        public void HitAttack() => OnHitAttack?.Invoke();
    }
}
