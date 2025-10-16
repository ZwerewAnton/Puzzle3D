using UnityEngine;

namespace Gameplay.Movement
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class DetailMovingObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshFilter meshFilter;
        
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        public void SetMesh(Mesh mesh, Material material)
        {
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
        }
    }
}