using System.Collections;
using Hugo.I.Scripts.Sounds;
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
        
        private Coroutine _isCoolingDownCoroutine;
        
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
                    _isCoolingDownCoroutine = StartCoroutine(CoolDown());
                }
            }
        }

        private void Start()
        {
            _currentCapacity = WeaponData.Capacity;
            CurrentOverheating = 0;
        }

        private void OnEnable()
        {
            if (CurrentOverheating >= 0)
            {
                _isCoolingDown = true;
                _isCoolingDownCoroutine = StartCoroutine(CoolDown());
            }
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(StartCollingDown));
            
            if (_isCoolingDownCoroutine != null)
            {
                StopCoroutine(_isCoolingDownCoroutine);
                _isCoolingDownCoroutine = null;
            }
        }

        public void Shoot(float readValue)
        {
            if (readValue > 0 && !_isShooting && _canShoot && _lastFireTime + WeaponData.FireRate < Time.time
                && CurrentCapacity > 0)
            {
                _isShooting = true;
                _isCoolingDown = false;
                StartCoroutine(Shooting());

                if (_isCoolingDownCoroutine != null)
                {
                    StopCoroutine(_isCoolingDownCoroutine);
                    _isCoolingDownCoroutine = null;
                }
            }
            else
            {
                _isShooting = false;
                
                if (CurrentOverheating >= 0)
                {
                    _isCoolingDown = true;
                    CancelInvoke(nameof(StartCollingDown));
                    Invoke(nameof(StartCollingDown), WeaponData.FireRate + 0.1f);
                }
            }
        }
        
        public void Reload()
        {
            CurrentCapacity += WeaponData.CapacityRestoreRate * Time.deltaTime;
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
                newBullet.GetComponent<BulletHandler>().SetUp(WeaponData.Damage, WeaponData.Range, WeaponData.BulletSpeed, _directionBulletTransform);
                
                // Sound
                SoundManager.Instance.PlaySound(gameObject, SoundManager.Instance.WeaponShootSounds);
                
                yield return new WaitForSeconds(WeaponData.FireRate);
            }
        }

        private void StartCollingDown()
        {
            if (_isCoolingDownCoroutine == null)
            {
                _isCoolingDownCoroutine = StartCoroutine(CoolDown());
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
                    CurrentOverheating = 0;
                    _isCoolingDown = false;
                    _isOverheating = false;
                    _canShoot = true;
                    
                    _isCoolingDownCoroutine = null;
                }
                
                yield return new WaitForSeconds(WeaponData.OverheatingDecreaseSpeed);
            }
        }

        public void ResetWeapon()
        {
            _currentCapacity = WeaponData.Capacity;
            CurrentOverheating = 0;
        }
    }
}
