using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuOrientationsManager : MonoBehaviour
{
    public RectTransform logoRectTransform;
    public Vector3 portraitTransformLogo;
    public Vector3 landscapeTransformLogo;
    private bool _isOrientationChange;
    private DeviceOrientation currentOrientation;
    private void Start() 
    {
        currentOrientation = Input.deviceOrientation;
    }
    private void Update() 
    {
        if(Input.deviceOrientation != currentOrientation)
        {
            _isOrientationChange = true;
        }

        if (_isOrientationChange)
        {
            currentOrientation = Input.deviceOrientation;
            if(currentOrientation == DeviceOrientation.Portrait){
                logoRectTransform.anchoredPosition = portraitTransformLogo;
            }
            else{
                logoRectTransform.anchoredPosition = landscapeTransformLogo;
            }
            _isOrientationChange = false;
            
        }
    }
}
