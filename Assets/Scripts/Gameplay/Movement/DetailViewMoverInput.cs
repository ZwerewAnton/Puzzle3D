using System;
using Cameras;
using Configs;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Movement
{
    public class DetailViewMoverInput : IDetailViewMoverInput, IDisposable
    {
        public event Action InputCanceled;
        
        private float _depth;
        private readonly InputHandler _inputHandler;
        private readonly Camera _camera;
        private readonly Vector3 _screenOffset;

        public DetailViewMoverInput(InputHandler inputHandler, CameraHandler cameraHandler, ApplicationConfigs configs)
        {
            _inputHandler = inputHandler;
            _inputHandler.DetailActions.Tap.canceled += OnTapCanceled;
            _camera = cameraHandler.Camera;
            _screenOffset = configs.gameplay.screenOffset;
        }

        public bool IsInputActive()
        {
            return _inputHandler.DetailActions.Tap.IsPressed();
        }

        public void UpdateDepth(Vector3 worldDepthPosition)
        {
            _depth = _camera.WorldToScreenPoint(worldDepthPosition).z;;
        }

        public Vector3 GetDesiredPosition()
        {
            return GetCursorWorldPoint();
        }

        public void Dispose()
        {
            _inputHandler.DetailActions.Tap.canceled -= OnTapCanceled;
        }

        private void OnTapCanceled(InputAction.CallbackContext callbackContext)
        {
            InputCanceled?.Invoke();
        }
        
        private Vector3 GetCursorWorldPoint()
        {
            Vector3 input = _inputHandler.DetailActions.Cursor.ReadValue<Vector2>();
            var cursor = input + _screenOffset;
            cursor.z = _depth;
            return _camera.ScreenToWorldPoint(cursor);
        }
    }
}