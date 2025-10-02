using UnityEngine;
using Zenject;

namespace Cameras
{
    public class OrbitCameraController : MonoBehaviour
    {
        private ICameraMovement _cameraMovement;

        [Inject]
        private void Construct(ICameraMovement cameraMovement)
        {
            _cameraMovement = cameraMovement;
        }

        private void LateUpdate()
        {
            UpdateCameraMovement();
        }

        private void UpdateCameraMovement()
        {
            var result = _cameraMovement.CalculateMovement();

            transform.rotation = result.Rotation;
            transform.position = result.Position;
        }
    }
}