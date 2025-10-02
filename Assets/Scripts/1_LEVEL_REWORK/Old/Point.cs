using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [Serializable]
    public class Point
    {
        [FormerlySerializedAs("Position")] public Vector3 position;
        [FormerlySerializedAs("Rotation")] public Vector3 rotation;
    }
}
