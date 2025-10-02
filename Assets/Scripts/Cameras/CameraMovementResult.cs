using UnityEngine;

namespace Cameras
{
    public struct CameraMovementResult
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public CameraMovementResult(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}