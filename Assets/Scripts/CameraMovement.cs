using UnityEngine;

public class CameraMovement : MonoBehaviour
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
 
    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private float _currentDistance;
    private float _desiredDistance;
    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;
    private Quaternion _rotation;
    private Vector3 _position;
    private Vector3 _prevMousePosition;
    private bool _isDragObject = true;
 
 
    void Start() { Init(); }
    void OnEnable() { Init(); }
 
    private void Init()
    {
        //Check and create target and camera object
        if (!target)
        {
            GameObject targetObject = new GameObject("TargetObject");
            targetObject.transform.position = transform.position + (cam.transform.forward * distance);
            target = targetObject.transform;
        }        
        if (!cam)
        {
            GameObject cameraObject = new GameObject("CameraObject");
            cameraObject.AddComponent<Camera>();
            //TODO Create a postion and rotaion for new camera. 
            //cameraObject.transform.position = transform.position + (cam.transform.forward * distance);
            cam = cameraObject.GetComponent<Camera>();
        }
        
        //distance = Vector3.Distance(transform.position, target.position);
        _currentDistance = distance;
        _desiredDistance = distance;
 
        //Get a starting point
        _position = cam.transform.position;
        _rotation = cam.transform.rotation;
        _currentRotation = cam.transform.rotation;
        _desiredRotation = cam.transform.rotation;
 
        _xDeg = Vector3.Angle(Vector3.right, cam.transform.right);
        _yDeg = Vector3.Angle(Vector3.up, cam.transform.up);
    }
 
    private void LateUpdate()
    {
        //TODO Make a touch control.
        //Scroll
        if (_isDragObject)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(_desiredDistance);
            }
            //Click
            if (Input.GetMouseButtonDown(0))
            {
                _prevMousePosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            //Drag
            if (Input.GetMouseButton(0))
            {
                Vector2 pos = _prevMousePosition - cam.ScreenToViewportPoint(Input.mousePosition);
                _xDeg -= pos.x * xSpeed;
                _yDeg += pos.y * ySpeed;
                _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
            }
        }

        //Orbit rotation of camera
        _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
        _currentRotation = cam.transform.rotation;
        _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * zoomDampening);
        cam.transform.rotation = _rotation;

        //Orbit position of camera
        _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * zoomDampening);
        _position = target.position - (_rotation * Vector3.forward * _currentDistance)  - targetOffset;
        cam.transform.position = _position;
        
        _prevMousePosition = cam.ScreenToViewportPoint(Input.mousePosition);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void RotateCameraOff()
    {
        _isDragObject = false;
    }     
    
    public void RotateCameraOn()
    {
        _isDragObject = true;
    } 
}
