using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Scriptable Objects/ResourceData")]
    public class ResourceData : ScriptableObject
    {
        public Utils.Resources ResourceType;
        public Sprite Icon;
        public int MaxCollectable;
    }
}
