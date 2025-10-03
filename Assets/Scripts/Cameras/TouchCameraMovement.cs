using Configs;
using Input;
using UnityEngine;
using Zenject;

namespace Cameras
{
    public class TouchCameraMovement : ICameraMovement
    {
        private readonly CameraConfigs _configs;
        private InputActions.CameraActions _cameraActions;
        
        private Vector2 _eulerAngles;
        private Vector2 _smoothedDelta;
        private Vector2 _rawDelta;

        private float _desiredDistance;
        private float _currentDistance;

        private bool _rotateAllowed;
        
        [Inject]
        public TouchCameraMovement(ApplicationConfigs configs, InputHandler inputHandler)
        {
            _configs = configs.camera;
            _cameraActions = inputHandler.CameraActions;
            InitializeParameters();
        }
        
        public CameraMovementResult CalculateMovement()
        {
            throw new System.NotImplementedException();
        }
        
        private void InitializeParameters()
        {
            _eulerAngles = _configs.startRotation.eulerAngles;
            _desiredDistance = _currentDistance = _configs.distance;
        }
    }
}