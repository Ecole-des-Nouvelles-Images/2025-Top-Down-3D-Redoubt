using UnityEngine;

namespace Hugo.I.Scripts.Weapon
{
    public class BulletHandler : MonoBehaviour 
    {
        private float _duration;
        private float _speed;
        private Vector3 _direction;
        
        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }

        public void SetUp(float duration, float speed, Transform direction)
        {
            _duration = duration;
            _speed = speed;
            _direction = (direction.position - transform.position).normalized;
            
            Destroy(gameObject, _duration);
        }
    }
}