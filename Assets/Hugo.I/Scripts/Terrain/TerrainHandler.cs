using Unity.AI.Navigation;
using UnityEngine;

namespace Hugo.I.Scripts.Terrain
{
    public class TerrainHandler : MonoBehaviour
    {
        private NavMeshSurface _navMeshSurface;

        private void Awake()
        {
            _navMeshSurface = GetComponent<NavMeshSurface>();
            _navMeshSurface.BuildNavMesh();
        }
    }
}
