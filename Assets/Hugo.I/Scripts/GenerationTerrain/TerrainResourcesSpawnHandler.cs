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
        
        [Header("Resources References")]
        [SerializeField] private List<GameObject> _stones;
        [SerializeField] private List<GameObject> _metals;
        [SerializeField] private List<GameObject> _electronics;
        
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
            foreach (var resourceSpawn in _resourceSpawns)
            {
                Dictionary<ResourcesEnum, List<Vector3>> dictionary = resourceSpawn.SpawnResources(_terrain);

                foreach (var keyValuePair in dictionary)
                {
                    if (keyValuePair.Key == ResourcesEnum.Stone)
                    {
                        foreach (var position in keyValuePair.Value)
                        {
                            Instantiate(_stones[Random.Range(0, _stones.Count)], position, Quaternion.identity);
                        }
                    }

                    if (keyValuePair.Key == ResourcesEnum.Metal)
                    {
                        foreach (var position in keyValuePair.Value)
                        {
                            Instantiate(_metals[Random.Range(0, _stones.Count)], position, Quaternion.identity);
                        }
                    }
                    
                    if (keyValuePair.Key == ResourcesEnum.ElectricalCircuit)
                    {
                        foreach (var position in keyValuePair.Value)
                        {
                            Instantiate(_electronics[Random.Range(0, _stones.Count)], position, Quaternion.identity);
                        }
                    }
                }
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
    }
}
