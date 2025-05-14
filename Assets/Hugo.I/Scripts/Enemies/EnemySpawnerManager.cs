using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.I.Scripts.Enemies
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private List<Transform> _spawnPointsT0;
        [SerializeField] private List<Transform> _spawnPointsT1;
        [SerializeField] private List<Transform> _spawnPointsT2;
        [SerializeField] private List<Transform> _spawnPointsT3;
        
        [Header("Settings")]
        [SerializeField] private int _totalCredit;
        
        private List<(Vector3, Vector3)> _edges = new List<(Vector3, Vector3)>();

        public bool IsSpawning = true;
        public int CurrentCredit;

        private void Awake()
        {
            CreateEdges(_spawnPointsT0);
        }

        private void Update()
        {
            if (CurrentCredit < _totalCredit && IsSpawning)
            {
                SpawnEnemy(Random.Range(CurrentCredit, _totalCredit + (int)(_totalCredit * 1.2f)));
            }
        }

        public void ChangeSpawnPoints(int towerLevel)
        {
            if (towerLevel == 1)
            {
                CreateEdges(_spawnPointsT1);
            }
            if (towerLevel == 2)
            {
                CreateEdges(_spawnPointsT2);
            }
            if (towerLevel == 3)
            {
                CreateEdges(_spawnPointsT3);
            }
        }

        private void CreateEdges(List<Transform> spawnPoints)
        {
            _edges.Clear();
            
            (Vector3, Vector3) edgeTop = (spawnPoints[0].position, spawnPoints[1].position);
            (Vector3, Vector3) edgeRight = (spawnPoints[1].position, spawnPoints[2].position);
            (Vector3, Vector3) edgeBot = (spawnPoints[2].position, spawnPoints[3].position);
            (Vector3, Vector3) edgeLeft = (spawnPoints[3].position, spawnPoints[0].position);
            
            _edges.Add(edgeTop);
            _edges.Add(edgeRight);
            _edges.Add(edgeBot);
            _edges.Add(edgeLeft);
        }

        private void SpawnEnemy(int number)
        {
            for (int i = 0; i < number; i++)
            {
                (Vector3, Vector3) _edge = _edges[Random.Range(0, _edges.Count)];
                Vector3 position = new Vector3(Random.Range(_edge.Item1.x, _edge.Item2.x), 0, Random.Range(_edge.Item1.z, _edge.Item2.z));
                Instantiate(_enemyPrefab, position, Quaternion.identity);
                CurrentCredit++;
            }
        }
    }
}