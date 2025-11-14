using UnityEngine;

namespace Cameras.Input
{
    public interface ICameraInputProvider
    {
        bool IsRotationAllowed { get; }
        Vector2 RotationDelta { get; }
        float ZoomDelta { get; }
        void UpdateInput();
    }
}