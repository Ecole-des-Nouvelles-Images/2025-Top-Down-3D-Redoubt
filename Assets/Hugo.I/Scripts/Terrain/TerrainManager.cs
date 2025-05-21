using System.Collections.Generic;
using System.Text;
using Hugo.I.Scripts.GenerationTerrain;
using Unity.AI.Navigation;
using UnityEngine;
using Random = System.Random;

namespace Hugo.I.Scripts.Terrain
{
    public class TerrainManager : MonoBehaviour
    {
        [Header("Seed Parameters")]
        [SerializeField] private string _mainSeed;
        [SerializeField] private int _seedLength;
        
        // Seed
        private Random _random;
        private string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        // Procedural
        private TerrainPropsSpawnerHandler[] _terrainPropsSpawners;
        private TerrainLevelingHandler _terrainLevelingHandler;
        
        // Internal Component
        private NavMeshSurface _navMeshSurface;
        
        private void Awake()
        {
            // Random
            _random = new Random();
            
            // Navmesh
            _navMeshSurface = GetComponent<NavMeshSurface>();
            Invoke(nameof(BakeNavMesh), 0.1f);
            
            _terrainPropsSpawners = GetComponents<TerrainPropsSpawnerHandler>();
            _terrainLevelingHandler = GetComponent<TerrainLevelingHandler>();
            
            // Generate MainSeed
            _mainSeed = GenerateDeterministicSeed(_random, _seedLength);
            
            // Generate and apply seed leveling
            List<string> childrenSeedsleveling =
                GenerateChildSeeds(_mainSeed, _terrainLevelingHandler.GetTerrainParametersCount());

            _terrainLevelingHandler.SetUp(childrenSeedsleveling);
            
            // Generate and apply seed props
            List<string> childrenSeedsProps = GenerateChildSeeds(_mainSeed, _terrainPropsSpawners.Length);
            
            for (int i = 0; i < _terrainPropsSpawners.Length; i++)
            {
                _terrainPropsSpawners[i].SetUp(childrenSeedsProps[i]);
            }
        }
        
        private string GenerateDeterministicSeed(Random random, int length = 16)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }

            return sb.ToString();
        }
        
        private List<string> GenerateChildSeeds(string masterSeed, int childCount)
        {
            var childSeeds = new List<string>();
            int masterHash = masterSeed.GetHashCode();

            var random = new Random(masterHash);

            for (int i = 0; i < childCount; i++)
            {
                string childSeed = GenerateDeterministicSeed(random);
                childSeeds.Add(childSeed);
            }

            return childSeeds;
        }

        private void BakeNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }
    }
}
