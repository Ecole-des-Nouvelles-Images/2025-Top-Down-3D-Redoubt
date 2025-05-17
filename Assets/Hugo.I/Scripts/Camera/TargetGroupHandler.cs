using Hugo.I.Scripts.Game;
using Unity.Cinemachine;
using UnityEngine;

namespace Hugo.I.Scripts.Camera
{
    public class TargetGroupHandler : MonoBehaviour
    {
        private CinemachineTargetGroup _targetGroup;

        private void Awake()
        {
            _targetGroup = GetComponent<CinemachineTargetGroup>();
        }

        private void Start()
        {
            foreach (GameObject player in GameManager.Instance.Players)
            {
                var newTarget = new CinemachineTargetGroup.Target
                {
                    Object = player.transform,
                    Weight = 1f,
                    Radius = 1f
                };
                
                _targetGroup.AddMember(newTarget.Object, newTarget.Weight, newTarget.Radius);
            }
        }
    }
}
