using System;

namespace Hugo.I.Scripts.Player
{
    public class PlayerEvents
    {
        public event Action<float> OnMove;
        public event Action<bool> OnHealing;
        public event Action<bool> OnReloading;
        public event Action OnCollecting;
        public event Action OnTakingDamage;
        public event Action OnPushing;

        public void Move(float velocity) => OnMove?.Invoke(velocity);
        public void Healing(bool isHealing) => OnHealing?.Invoke(isHealing);
        public void Reloading(bool isReloading) => OnReloading?.Invoke(isReloading);
        public void Collecting() => OnCollecting?.Invoke();
        public void TakingDamage() => OnTakingDamage?.Invoke();
        public void Pushing() => OnPushing?.Invoke();
    }
}
