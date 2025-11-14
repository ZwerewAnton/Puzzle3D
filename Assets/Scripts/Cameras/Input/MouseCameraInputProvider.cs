using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Cameras.Input
{
    public class MouseCameraInputProvider : ICameraInputProvider
    {
        private const float ScrollCoefficient = 0.001f;
        private readonly InputActions.CameraActions _actions;

        public bool IsRotationAllowed { get; private set; }
        public Vector2 RotationDelta { get; private set; }
        public float ZoomDelta { get; private set; }

        [Inject]
        public MouseCameraInputProvider(InputHandler inputHandler)
        {
            _actions = inputHandler.CameraActions;
        }

        public void UpdateInput()
        {
            if (_actions.Press.WasPressedThisFrame())
                IsRotationAllowed = !EventSystem.current.IsPointerOverGameObject();

            if (_actions.Press.IsPressed() && IsRotationAllowed)
                RotationDelta = _actions.Look.ReadValue<Vector2>();
            else
                RotationDelta = Vector2.zero;

            ZoomDelta = _actions.Zoom.ReadValue<float>() * ScrollCoefficient;
        }
    }
}