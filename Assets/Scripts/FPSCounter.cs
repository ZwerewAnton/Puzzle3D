using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] 
    private Text _fpsText;
    [SerializeField] 
    private Text _dpiText;
    [SerializeField] 
    private Text _eventText;
    [SerializeField] 
    private float _hudRefreshRate = 1f;
 
    private float _timer;
    private void Start(){
        _dpiText.text = "PPI: " + Screen.dpi.ToString();
        
    }
    private void Update()
    {
        
        if(Input.touchCount > 0){

            _eventText.text = Input.GetTouch(0).deltaPosition.ToString() + " " + Input.GetTouch(0).phase.ToString();
            Debug.Log(Input.GetTouch(0).deltaPosition);
            Debug.Log(Input.GetTouch(0).deltaPosition.magnitude);
        }
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}
