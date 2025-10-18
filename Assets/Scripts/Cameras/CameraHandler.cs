using UnityEngine;

namespace Cameras
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private Camera sceneCamera;

        public Camera Camera => sceneCamera;
    }
}