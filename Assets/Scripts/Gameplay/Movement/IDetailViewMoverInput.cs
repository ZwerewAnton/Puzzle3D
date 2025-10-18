using System;
using UnityEngine;

namespace Gameplay.Movement
{
    public interface IDetailViewMoverInput
    {
        event Action InputCanceled;
        bool IsInputActive();
        void UpdateDepth(Vector3 worldDepthPosition);
        Vector3 GetDesiredPosition();
    }
}