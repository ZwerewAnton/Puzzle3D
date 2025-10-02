using Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Cameras
{
    public class MouseCameraMovement : ICameraMovement
    {
        private readonly CameraConfigs _configs;
        
        private Vector2 _eulerAngles;
        private Vector2 _smoothedDelta;

        private float _desiredDistance;
        private float _currentDistance;

        [Inject]
        public MouseCameraMovement(ApplicationConfigs configs)
        {
            _configs = configs.camera;
            InitializeParameters();
        }

        public CameraMovementResult CalculateMovement()
        {
            if (UnityEngine.Input.GetMouseButton(1))
            {
                var rawDelta = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));

                _smoothedDelta = Vector2.Lerp(_smoothedDelta, rawDelta, Time.deltaTime * _configs.mouseSmooth);

                _eulerAngles.y += _smoothedDelta.x * _configs.xSpeed * Time.deltaTime;
                _eulerAngles.x -= _smoothedDelta.y * _configs.ySpeed * Time.deltaTime;
                _eulerAngles.x = ClampAngle(_eulerAngles.x, _configs.yMinLimit, _configs.yMaxLimit);
            }
            else
            {
                _smoothedDelta = Vector2.zero;
            }

            var maxDistance = _configs.maxDistance;
            var minDistance = _configs.minDistance;
            var scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            _desiredDistance -= scroll * (maxDistance - minDistance) * _configs.zoomSpeed;
            _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);

            _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * _configs.zoomDampening);

            var rotation = Quaternion.Euler(_eulerAngles.x, _eulerAngles.y, 0);
            var negativeDistance = new Vector3(0.0f, 0.0f, -_currentDistance);
            var position = rotation * negativeDistance + _configs.targetOffset;
            return new CameraMovementResult(position, rotation);
        }

        private void InitializeParameters()
        {
            _eulerAngles = _configs.startRotation.eulerAngles;
            _desiredDistance = _currentDistance = _configs.distance;
        }
        
        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}