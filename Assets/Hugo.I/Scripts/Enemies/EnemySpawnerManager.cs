using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private List<Transform> _spawnPoints;
        
        [Header("Settings")]
        [SerializeField] private int _totalCredit;
        
        private List<(Vector3, Vector3)> _edges = new List<(Vector3, Vector3)>();

        private void Awake()
        {
            (Vector3, Vector3) edgeTop = (_spawnPoints[0].position, _spawnPoints[1].position);
            (Vector3, Vector3) edgeRight = (_spawnPoints[1].position, _spawnPoints[2].position);
            (Vector3, Vector3) edgeBot = (_spawnPoints[2].position, _spawnPoints[3].position);
            (Vector3, Vector3) edgeLeft = (_spawnPoints[3].position, _spawnPoints[0].position);
            
            _edges.Add(edgeTop);
            _edges.Add(edgeRight);
            _edges.Add(edgeBot);
            _edges.Add(edgeLeft);
        }

        private void Start()
        {
            SpawnEnemy(_totalCredit);
        }

        public void SpawnEnemy(int number)
        {
            for (int i = 0; i < number; i++)
            {
                (Vector3, Vector3) _edge = _edges[Random.Range(0, _edges.Count)];
                Vector3 position = new Vector3(Random.Range(_edge.Item1.x, _edge.Item2.x), 0, Random.Range(_edge.Item1.z, _edge.Item2.z));
                Instantiate(_enemyPrefab, position, Quaternion.identity);
            }
        }
    }
}