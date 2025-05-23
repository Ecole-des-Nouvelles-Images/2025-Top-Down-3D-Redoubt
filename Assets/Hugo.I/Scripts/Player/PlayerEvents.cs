using System;

namespace Hugo.I.Scripts.Player
{
    public class PlayerEvents
    {
        public event Action<float> OnMove;
        public event Action OnHealing;
        public event Action OnReloading;

        public void Move(float velocity) => OnMove?.Invoke(velocity);
        public void Healing() => OnHealing?.Invoke();
        public void Reloading() => OnReloading?.Invoke();
    }
}
