using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Weapon
{
    public class BulletHandler : MonoBehaviour
    {
        private float _damage;
        private float _duration;
        private float _speed;
        private Vector3 _direction;

        private Vector3 _lastPosition;
        
        private void Update()
        {
            CheckCollision(transform.position);
            
            transform.position += _direction * (_speed * Time.deltaTime);
        }

        public void SetUp(float damage, float duration, float speed, Transform direction)
        {
            _damage = damage;
            _duration = duration;
            _speed = speed;
            _direction = (direction.position - transform.position).normalized;
            
            _lastPosition = transform.position;
            
            Destroy(gameObject, _duration);
        }

        private void CheckCollision(Vector3 actualPosition)
        {
            int layerMask = LayerMask.GetMask("HitBullet");
            Vector3 direction = (_lastPosition - actualPosition).normalized;
            float distance = Vector3.Distance(actualPosition, _lastPosition);
            
            RaycastHit[] hits = Physics.RaycastAll(actualPosition, direction, distance, layerMask);

            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        hit.collider.gameObject.GetComponent<IHaveHealth>().TakeDamage(_damage);
                    }
                    Destroy(gameObject);
                }
            }
            
            _lastPosition = actualPosition;
        }
    }
}