using UnityEngine;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _enemySpawnPosition;

        private void Start()
        {
            Instantiate(_enemyPrefab, _enemySpawnPosition);
        }
    }
}
