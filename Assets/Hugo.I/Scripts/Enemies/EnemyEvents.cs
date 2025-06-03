using System;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemyEvents
    {
        public event Action OnTakeDamage;
        
        // Animation
        public event Action OnFootStep;
        public event Action OnHitAttack;
        
        public void TakeDamage() => OnTakeDamage?.Invoke();
        public void FootStep() => OnFootStep?.Invoke();
        public void HitAttack() => OnHitAttack?.Invoke();
    }
}
