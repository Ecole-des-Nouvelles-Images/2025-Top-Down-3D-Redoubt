using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Hugo.I.Scripts.Player
{
    public class PlayerTwoBonesIkHandler : MonoBehaviour
    {
        [Header("References")]
        [Header("Weapons")]
        [SerializeField] private Rig _rightHandHoldRevolver;
        [SerializeField] private Rig _leftHandHoldRevolver;
        [SerializeField] private Rig _rightHandHoldRiffle;
        [SerializeField] private Rig _leftHandHoldRiffle;
        
        [Header("Shield")]
        [SerializeField] private Rig _rightHandHoldShield;

        public void EnableTwoBonesIkWeapons(WeaponData weapon)
        {
            if (weapon._weaponTypesEnum == WeaponTypesEnum.Revolver)
            {
                _rightHandHoldRiffle.weight = 0;
                _leftHandHoldRiffle.weight = 0;
                
                _rightHandHoldRevolver.weight = 1;
                _leftHandHoldRevolver.weight = 1;
            }
            else
            {
                _rightHandHoldRiffle.weight = 1;
                _leftHandHoldRiffle.weight = 1;
                
                _rightHandHoldRevolver.weight = 0;
                _leftHandHoldRevolver.weight = 0;
            }
        }

        public void EnableTwoBonesIkShield()
        {
            _rightHandHoldShield.weight = 1;
        }

        public void DisableTwoBonesIk()
        {
            Debug.Log("Disable Two Bones Ik");
            _rightHandHoldRiffle.weight = 0;
            _leftHandHoldRiffle.weight = 0;
            _rightHandHoldRevolver.weight = 0;
            _leftHandHoldRevolver.weight = 0;
            _rightHandHoldShield.weight = 0;
        }
    }
}