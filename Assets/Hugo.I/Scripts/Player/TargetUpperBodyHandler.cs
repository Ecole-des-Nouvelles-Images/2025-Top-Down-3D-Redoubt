using UnityEngine;

namespace Hugo.I.Scripts.Player
{
    public class TargetUpperBodyHandler : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void Update()
        {
            if (_target == null) return;

            Vector3 directionToTarget = _target.position - transform.position;
            Vector3 oppositeDirection = -directionToTarget;

            oppositeDirection.y = 0;

            if (oppositeDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(oppositeDirection);
        }
    }
}
