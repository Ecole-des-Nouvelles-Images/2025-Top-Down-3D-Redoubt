namespace Hugo.I.Scripts.Utils
{
    public interface IResource {
        public Resources GetResourceType();
        public int GetResourceMaxCollectable();
        public void OnEnterZone();
        public void OnExitZone();
    }
}
