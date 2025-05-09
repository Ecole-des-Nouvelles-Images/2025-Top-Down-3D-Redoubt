using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.I.Scripts.Weapon
{
    public class WeaponHandler : MonoBehaviour
    {
        [FormerlySerializedAs("_weaponData")] public WeaponData WeaponData;
        [SerializeField] private GameObject _bulletGameObject;
        [SerializeField] private Transform _spawnBulletTransform;
        [SerializeField] private Transform _directionBulletTransform;

        private bool _canShoot = true;
        private bool _isShooting;
        private bool _isCoolingDown;
        private bool _isOverheating;
        private bool _isReloading = false;
        
        private float _currentCapacity;
        private float _currentOverheating;
        private float _lastFireTime;
        
        public float CurrentCapacity
        {
            get => _currentCapacity;
            set
            {
                _currentCapacity = value;
                _currentCapacity = Mathf.Clamp(value, 0f, WeaponData.Capacity);
                _canShoot = _currentCapacity > 0;
            }
        }
        public float CurrentOverheating
        {
            get => _currentOverheating;
            set
            {
                _currentOverheating = value;
                if (_currentOverheating >= WeaponData.OverheatingLimit && !_isOverheating)
                {
                    _isOverheating = true;
                    _isCoolingDown = true;
                    _canShoot = false;
                    StartCoroutine(CoolDown());
                }
            }
        }

        private void Start()
        {
            _currentCapacity = WeaponData.Capacity;
            _currentOverheating = 0;
        }

        private void OnEnable()
        {
            if (_currentOverheating >= 0)
            {
                _isCoolingDown = true;
                StartCoroutine(CoolDown());
            }
        }

        public void Shoot(float readValue)
        {
            if (readValue > 0 && !_isShooting && _canShoot && _lastFireTime + WeaponData.FireRate < Time.time)
            {
                _isShooting = true;
                StartCoroutine(Shooting());
            }
            else
            {
                _isShooting = false;
                
                if (CurrentOverheating >= 0)
                {
                    _isCoolingDown = true;
                    StartCoroutine(CoolDown());
                }
            }
        }
        
        public void Reload()
        {
            CurrentCapacity += WeaponData.CapacityRestoreRate + Time.deltaTime;
        }
        
        private IEnumerator Shooting()
        {
            while (_isShooting && _canShoot)
            {
                //Datas
                CurrentCapacity--;
                CurrentOverheating += WeaponData.OverheatingIncreaseRate;
                _lastFireTime = Time.time;
                
                //Visuel
                GameObject newBullet = Instantiate(_bulletGameObject, _spawnBulletTransform.position, transform.rotation);
                newBullet.GetComponent<BulletHandler>().SetUp(WeaponData.Range, WeaponData.BulletSpeed, _directionBulletTransform);
                
                yield return new WaitForSeconds(WeaponData.FireRate);
            }
        }

        private IEnumerator CoolDown()
        {
            while (_isCoolingDown && !_isShooting)
            {
                //Datas
                CurrentOverheating -= WeaponData.OverheatingDecreaseRate;
                
                if (CurrentOverheating <= 0)
                {
                    _currentOverheating = 0;
                    _isCoolingDown = false;
                    _isOverheating = false;
                    _canShoot = true;
                }
                
                yield return new WaitForSeconds(WeaponData.OverheatingDecreaseSpeed);
            }
        }
    }
}
