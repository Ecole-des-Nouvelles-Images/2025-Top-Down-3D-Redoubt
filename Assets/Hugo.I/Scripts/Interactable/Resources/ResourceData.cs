using UnityEngine;

namespace Hugo.I.Scripts.Interactable.Resources
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Scriptable Objects/ResourceData")]
    public class ResourceData : ScriptableObject
    {
        public Utils.ResourcesEnum ResourceEnumType;
        public Sprite Icon;
        public int MaxCollectableAtOnce;
    }
}
