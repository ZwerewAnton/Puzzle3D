using UnityEngine;

namespace Cameras
{
    public interface ICameraMovement
    {
        public CameraMovementResult CalculateMovement();
    }
}