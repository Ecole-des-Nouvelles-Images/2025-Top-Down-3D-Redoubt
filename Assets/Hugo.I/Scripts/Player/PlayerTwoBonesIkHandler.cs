using Hugo.I.Scripts.Utils;
using Hugo.I.Scripts.Weapon;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Hugo.I.Scripts.Player
{
    public class PlayerTwoBonesIkHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rig _rightHandHoldRevolver;
        [SerializeField] private Rig _leftHandHoldRevolver;
        [SerializeField] private Rig _rightHandHoldRiffle;
        [SerializeField] private Rig _leftHandHoldRiffle;

        public void EnableTwoBonesIk(WeaponData weapon)
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

        public void DisableTwoBonesIk()
        {
            Debug.Log("Disable Two Bones Ik");
            _rightHandHoldRiffle.weight = 0;
            _leftHandHoldRiffle.weight = 0;
            _rightHandHoldRevolver.weight = 0;
            _leftHandHoldRevolver.weight = 0;
        }
    }
}