using UnityEngine;
using Zenject;

namespace Cameras
{
    public class OrbitCameraController : MonoBehaviour
    {
        private ICameraMovement _cameraMovement;
        private Transform _cameraTransform;

        [Inject]
        private void Construct(ICameraMovement cameraMovement, CameraHandler cameraHandler)
        {
            _cameraMovement = cameraMovement;
            _cameraTransform = cameraHandler.transform;
        }

        private void LateUpdate()
        {
            UpdateCameraMovement();
        }

        private void UpdateCameraMovement()
        {
            var result = _cameraMovement.CalculateMovement();

            _cameraTransform.rotation = result.Rotation;
            _cameraTransform.position = result.Position;
        }
    }
}