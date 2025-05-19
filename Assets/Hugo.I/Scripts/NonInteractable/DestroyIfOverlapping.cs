using UnityEngine;

namespace Hugo.I.Scripts.NonInteractable
{
    public class DestroyIfOverlapping : MonoBehaviour
    {
        [SerializeField] private float _overlapRadius;
        [SerializeField] private LayerMask _overlapLayers;

        private void Start()
        {
            Collider[] overlaps = Physics.OverlapSphere(transform.position, _overlapRadius, _overlapLayers);

            foreach (Collider col in overlaps)
            {
                if (col.gameObject != gameObject)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _overlapRadius);
        }
    }
}
