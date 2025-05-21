using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.GenerationTerrain
{
    public class TerrainResourcesSpawnHandler : MonoBehaviour
    {
        [Header("Spawning Points Settings")]
        [SerializeField] private List<ResourceParameters> _resourceSpawns;
        [SerializeField] private List<GameObject> _parentGameObjectToSpawnResources;
        [SerializeField] private Transform _resourcesParent;
        
        [Header("Prefab References")]
        [SerializeField] private List<GameObject> _stones;
        [SerializeField] private List<GameObject> _metals;
        [SerializeField] private List<GameObject> _electronics;
        [SerializeField] private GameObject _powerPlant;
        
        private UnityEngine.Terrain _terrain;

        private void Awake()
        {
            _terrain = GetComponent<UnityEngine.Terrain>();

            int index = 0;
            foreach (var parent in _parentGameObjectToSpawnResources)
            {
                _resourceSpawns[index].SetSpawningPoints(GetAllChildTransforms(parent));
                index++;
            }
        }

        private void Start()
        {
            Invoke(nameof(SpawnResources), 0.1f);
        }

        private void SpawnResources()
        {
            foreach (var resourceSpawn in _resourceSpawns)
            {
                Dictionary<ResourcesEnum, List<Vector3>> dictionary = resourceSpawn.SpawnResources(_terrain);

                foreach (var keyValuePair in dictionary)
                {
                    if (keyValuePair.Key == ResourcesEnum.Stone)
                    {
                        foreach (var position in keyValuePair.Value)
                        {
                            Vector3 eulerRotation = new Vector3(0f, Random.Range(-30f, 30f), 0f);
                            Quaternion rotation = Quaternion.Euler(eulerRotation);
                            GameObject obj = Instantiate(_stones[Random.Range(0, _stones.Count)], position, rotation, _resourcesParent);
                        }
                    }

                    if (keyValuePair.Key == ResourcesEnum.Metal)
                    {
                        foreach (var position in keyValuePair.Value)
                        {
                            Vector3 eulerRotation = new Vector3(0f, Random.Range(-30f, 30f), 0f);
                            Quaternion rotation = Quaternion.Euler(eulerRotation);
                            GameObject obj = Instantiate(_metals[Random.Range(0, _metals.Count)], position, rotation, _resourcesParent);
                        }
                    }
                    
                    if (keyValuePair.Key == ResourcesEnum.ElectricalCircuit)
                    {
                        foreach (var position in keyValuePair.Value)
                        {
                            Vector3 eulerRotation = new Vector3(0f, Random.Range(-30f, 30f), 0f);
                            Quaternion rotation = Quaternion.Euler(eulerRotation);
                            GameObject obj = Instantiate(_electronics[Random.Range(0, _electronics.Count)], position, rotation, _resourcesParent);
                        }
                    }
                }
            }
            
            // Spawn Power Plant
            if (_powerPlant)
            {
                Transform randomChild = GetRandomChild(_parentGameObjectToSpawnResources[1]);
                Vector3 eulerRotation = new Vector3(0f, Random.Range(-30f, 30f), 0f);
                Quaternion rotation = Quaternion.Euler(eulerRotation);
                
                GameObject obj = Instantiate(_powerPlant, randomChild.position, rotation, _resourcesParent);
            }
        }
        
        private List<Transform> GetAllChildTransforms(GameObject parent)
        {
            List<Transform> children = new List<Transform>();
    
            foreach (Transform child in parent.transform)
            {
                children.Add(child);
            }

            return children;
        }
        
        private Transform GetRandomChild(GameObject parent)
        {
            int childCount = parent.transform.childCount;
            if (childCount == 0) return null;

            int randomIndex = Random.Range(0, childCount);
            return parent.transform.GetChild(randomIndex);
        }

    }
}
