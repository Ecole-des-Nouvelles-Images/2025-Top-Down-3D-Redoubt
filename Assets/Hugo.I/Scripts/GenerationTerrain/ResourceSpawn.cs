using System;
using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.I.Scripts.GenerationTerrain
{
    [Serializable]
    public class ResourceSpawn
    {
        [SerializeField] private string _zone;
        [SerializeField] private List<ResourceSpawnType> _resourceSpawnType;
        
        private List<Transform> _spawningPoints;

        public void SetSpawningPoints(List<Transform> spawningPoints)
        {
            _spawningPoints = spawningPoints;
        }
        
        public Dictionary<ResourcesEnum, List<Vector3>> SpawnResources(UnityEngine.Terrain terrain)
        {
            Dictionary<ResourcesEnum, List<Vector3>> dictionary = new Dictionary<ResourcesEnum, List<Vector3>>();
            
            foreach (var resourceSpawnType in _resourceSpawnType)
            {
                dictionary.Add(resourceSpawnType.Resource, new List<Vector3>());
            }
            
            foreach (var resource in _resourceSpawnType)
            {
                for (int i = 0; i < resource.NumberResourceToSpawn; i++)
                {
                    int index = Random.Range(0, _spawningPoints.Count);
                    
                    float posY = terrain.SampleHeight(_spawningPoints[index].position) + terrain.GetPosition().y;
                    Vector3 spawningPosition = new Vector3(_spawningPoints[index].position.x, posY, _spawningPoints[index].position.z);
                    
                    dictionary[resource.Resource].Add(spawningPosition);
                    _spawningPoints.RemoveAt(index);
                }
            }
            
            return dictionary;
        }
    }
}