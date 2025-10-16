
using UnityEngine;

namespace _1_LEVEL_REWORK.New.Instances
{
    public struct PointTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public PointTransform(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}