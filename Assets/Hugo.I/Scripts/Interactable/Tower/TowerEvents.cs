using System;

namespace Hugo.I.Scripts.Interactable.Tower
{
    public class TowerEvents
    {
        public event Action OnUpgrading;
        
        public void Upgrading() => OnUpgrading?.Invoke();
    }
}
