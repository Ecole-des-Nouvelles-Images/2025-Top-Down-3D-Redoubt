using System;

namespace Hugo.I.Scripts.Player
{
    public class PlayerEvents
    {
        // Player Events
        public event Action<bool> OnHealing;
        public event Action<bool> OnReloading;
        public event Action OnCollecting;
        public event Action OnTakingDamage;

        // Animation Events
        public event Action OnHitPush;
        public event Action OnFootStep;

        public void Healing(bool isHealing) => OnHealing?.Invoke(isHealing);
        public void Reloading(bool isReloading) => OnReloading?.Invoke(isReloading);
        public void Collecting() => OnCollecting?.Invoke();
        public void TakingDamage() => OnTakingDamage?.Invoke();
        public void HitPush() => OnHitPush?.Invoke();
        public void FootStep() => OnFootStep?.Invoke();
    }
}
