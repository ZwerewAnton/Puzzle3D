using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator1 : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    
    private Vector3 _localRotation;
    private Transform _transform;
    private Vector3 _previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        cam.transform.position = target.position;
        cam.transform.Translate(new Vector3(0, 0, -20));
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            //cam.transform.
            
            Vector3 direction = _previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);
            cam.transform.position = target.position;
            //var position = cam.transform.position;
            cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            //cam.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            cam.transform.Translate(new Vector3(0, 0, -20));

            //Debug.Log(-direction.x * 180);
            _previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);

        }
    }
}
