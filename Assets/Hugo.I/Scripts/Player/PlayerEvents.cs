using System;

namespace Hugo.I.Scripts.Player
{
    public class PlayerEvents
    {
        public event Action<float> OnMove;
        public event Action<bool> OnHealing;
        public event Action<bool> OnReloading;

        public void Move(float velocity) => OnMove?.Invoke(velocity);
        public void Healing(bool isHealing) => OnHealing?.Invoke(isHealing);
        public void Reloading(bool isReloading) => OnReloading?.Invoke(isReloading);
    }
}
