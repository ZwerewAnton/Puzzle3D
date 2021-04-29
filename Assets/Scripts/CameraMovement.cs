﻿using UnityEngine;
using UnityEngine.EventSystems;

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
    private Vector3 _downClickPosition;
    private bool _isClicked;
    private int currentFingerId;
 
 
    void Start() { Init(); }
 
    private void Init()
    {
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
            cam = cameraObject.GetComponent<Camera>();
        }
        
        _currentDistance = distance;
        _desiredDistance = distance;
 
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
        #if UNITY_EDITOR    
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(_desiredDistance);
            }
            if (Input.GetMouseButtonDown(0))
            {
                _isClicked = true;
                _downClickPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0) && _isClicked)
            {
                Vector2 pos = _downClickPosition - cam.ScreenToViewportPoint(Input.mousePosition);
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
        if(Input.touchSupported)
        {
            if(Input.touchCount == 1){
                if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Touch touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Began){
                            
                        _isClicked = true;
                        _downClickPosition = cam.ScreenToViewportPoint(touch.position);
                        currentFingerId = touch.fingerId;
                    }
                    else if (touch.fingerId == currentFingerId && touch.phase == TouchPhase.Moved && _isClicked/*  && touch.deltaPosition.magnitude >= 2f */) 
                    {
                        Vector2 pos = _downClickPosition - cam.ScreenToViewportPoint(touch.position);
                        _xDeg -= pos.x * xSpeed;
                        _yDeg += pos.y * ySpeed;
                        _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
                    }
                }
                else{
                    _isClicked = false;
                }


            }
            else if(Input.touchCount == 2)
            {
                Debug.Log("2");
                Touch tFirst = Input.GetTouch(0);
                Touch tSecond = Input.GetTouch(1);
                
                Vector2 tZeroPrevious = tFirst.position - tFirst.deltaPosition;
                Vector2 tOnePrevious = tSecond.position - tSecond.deltaPosition;
                
                float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance (tFirst.position, tSecond.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                _desiredDistance -= deltaDistance * zoomRate;

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
            
            _downClickPosition = cam.ScreenToViewportPoint(Input.GetTouch(0).position);

        }

        //Orbit rotation of camera
/*         _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
        _currentRotation = cam.transform.rotation;
        _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * zoomDampening);
        cam.transform.rotation = _rotation;

        //Orbit position of camera
        _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * zoomDampening);
        _position = target.position - (_rotation * Vector3.forward * _currentDistance)  - targetOffset;
        cam.transform.position = _position;
        
        _downClickPosition = cam.ScreenToViewportPoint(Input.mousePosition); */
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
