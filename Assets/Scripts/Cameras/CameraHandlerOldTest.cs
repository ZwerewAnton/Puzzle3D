using UnityEngine;
using UnityEngine.EventSystems;

namespace Cameras
{
    public class CameraHandlerOldTest : MonoBehaviour
    {
        [Tooltip("Target for a camera.")]
        public Transform target;
        public Camera cam;
        public Vector3 targetOffset;
        public float distance = 5.0f;
        public float maxDistance = 20;
        public float minDistance = .6f;
        public float xSpeed = 5.0f;
        public float ySpeed = 5.0f;
        public int yMinLimit = -80;
        public int yMaxLimit = 80;
        public float zoomRate = 10.0f;
        public float zoomDampening = 5.0f;
        public float doubleTapTime = 0.5f;
 
        private float _xDeg;
        private float _yDeg;
        private float _currentDistance;
        private float _desiredDistance;
        private Quaternion _currentRotation;
        private Quaternion _desiredRotation;
        private Quaternion _rotation;
        private Vector3 _position;
        private Vector3 _downClickPosition;
        private bool _isClicked;
        private int _currentFingerId;
        private int _tapCount;
        private float _newTime;

        private void Start()
        {
            Init();
        }
 
        private void Init()
        {
            if (!target)
            {
                var targetObject = new GameObject("TargetObject")
                {
                    transform =
                    {
                        position = transform.position + (cam.transform.forward * distance)
                    }
                };
                target = targetObject.transform;
            }
            if (!cam)
            {
                var cameraObject = new GameObject("CameraObject");
                cameraObject.AddComponent<Camera>();
                cam = cameraObject.GetComponent<Camera>();
            }
        
            _currentDistance = distance;
            _desiredDistance = distance;
 
            _position = cam.transform.position;
            _rotation = cam.transform.rotation;
            _currentRotation = cam.transform.rotation;
            _desiredRotation = cam.transform.rotation;
            _tapCount = 0;
 
            _xDeg = Vector3.Angle(Vector3.right, cam.transform.right);
            _yDeg = Vector3.Angle(Vector3.up, cam.transform.up);
        }
 
        private void LateUpdate()
        {
#if UNITY_EDITOR    
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") != 0f)
                {
                    _desiredDistance -= UnityEngine.Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(_desiredDistance);
                }
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    _isClicked = true;
                    _downClickPosition = cam.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
                }
                if (UnityEngine.Input.GetMouseButton(0) && _isClicked)
                {
                    Vector2 pos = _downClickPosition - cam.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
                    _xDeg -= pos.x * xSpeed;
                    _yDeg += pos.y * ySpeed;
                    _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
                }
            }
            else
            {
                _isClicked = false;
            }
#endif
            if (!UnityEngine.Input.touchSupported) 
                return;
            
            switch (UnityEngine.Input.touchCount)
            {
                case 1:
                {
                    var touch = UnityEngine.Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {        
                        _isClicked = true;
                        _downClickPosition = cam.ScreenToViewportPoint(touch.position);
                        _currentFingerId = touch.fingerId;
                    }
                    else if (touch.fingerId == _currentFingerId && touch.phase == TouchPhase.Moved && _isClicked) 
                    {
                        Vector2 pos = _downClickPosition - cam.ScreenToViewportPoint(touch.position);
                        _xDeg -= pos.x * xSpeed;
                        _yDeg += pos.y * ySpeed;
                        _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        _isClicked = false;
                        _tapCount += 1;
                    }

                    switch (_tapCount)
                    {
                        case 1:
                            _newTime = Time.time + doubleTapTime;
                            break;
                        case 2 when Time.time <= _newTime:
                            _tapCount = 0;
                            break;
                    }
            
                    _downClickPosition = cam.ScreenToViewportPoint(UnityEngine.Input.GetTouch(0).position);
                    break;
                }
                case 2:
                {
                    var touchZero = UnityEngine.Input.GetTouch(0);
                    var touchOne = UnityEngine.Input.GetTouch(1);
                    if (!EventSystem.current.IsPointerOverGameObject(touchZero.fingerId) && 
                        !EventSystem.current.IsPointerOverGameObject(touchOne.fingerId))
                    {
                        var tZeroPrevious = touchZero.position - touchZero.deltaPosition;
                        var tOnePrevious = touchOne.position - touchOne.deltaPosition;

                        var prevTouchDeltaMag = (tZeroPrevious - tOnePrevious).magnitude;
                        var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                        var deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
    
                        _desiredDistance += deltaMagDiff * Time.deltaTime * zoomRate * 0.0025f * Mathf.Abs(_desiredDistance);
                    }

                    break;
                }
            }
        
            if (Time.time > _newTime) 
            {
                _tapCount = 0;
            }
            _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
            _currentRotation = cam.transform.rotation;
            _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * zoomDampening);
            
            cam.transform.rotation = _rotation;

            //Orbit position of camera
            _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
            _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * zoomDampening);
            _position = target.position - (_rotation * Vector3.forward * _currentDistance)  - targetOffset;
            
            cam.transform.position = _position;
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