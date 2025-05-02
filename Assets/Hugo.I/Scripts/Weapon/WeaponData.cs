using Hugo.I.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.I.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [FormerlySerializedAs("_weaponTypeEnum")] [FormerlySerializedAs("WeaponType")] public WeaponTypesEnum _weaponTypesEnum;
        public float Damage;
        public float Capacity;
        public float FireRate;
        public float Range;
        public float BulletSpeed;
        public float OverheatingLimit;
        public float OverheatingIncreaseRate;
        public float OverheatingDecreaseRate;
        public float OverheatingDecreaseSpeed;
    }
}
