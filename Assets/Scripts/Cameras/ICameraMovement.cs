using UnityEngine;

namespace Cameras
{
    public interface ICameraMovement
    {
        CameraMovementResult CalculateMovement();
    }
}