using UnityEngine;

namespace _1_LEVEL_REWORK.New.Instances
{
    public class DetailPrefab : MonoBehaviour
    {
        public MeshRenderer MeshRenderer { get; private set; }

        private void Awake()
        {
            MeshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }
}