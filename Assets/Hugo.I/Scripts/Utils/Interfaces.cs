namespace Hugo.I.Scripts.Utils
{
    public interface IInteractable {
        public void OnEnterZone();
        public void OnExitZone();
    }

    public interface IHaveHealth {
        public void TakeDamage(float damage);
    }
}
