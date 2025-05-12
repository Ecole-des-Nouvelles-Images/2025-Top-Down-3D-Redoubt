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
        
        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }

        public void SetUp(float damage, float duration, float speed, Transform direction)
        {
            _damage = damage;
            _duration = duration;
            _speed = speed;
            _direction = (direction.position - transform.position).normalized;
            
            Destroy(gameObject, _duration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<IHaveHealth>().TakeDamage(_damage);
            }
        }
    }
}