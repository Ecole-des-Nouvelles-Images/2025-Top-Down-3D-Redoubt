using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.I.Scripts.Interactable.Resources
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Scriptable Objects/ResourceData")]
    public class ResourceData : ScriptableObject
    {
        [FormerlySerializedAs("ResourceType")] public Utils.ResourcesEnum _resourceEnumType;
        public Sprite Icon;
        public int MaxCollectable;
    }
}
