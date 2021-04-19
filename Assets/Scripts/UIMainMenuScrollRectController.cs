using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuScrollRectController : MonoBehaviour
{
    [Range(1, 50)]
    [Header("Controllers")]
    public int panCount;
    [Range(0, 500)]
    public int panOffset;
    [Range(0f, 20f)]
    public float snapSpeed;
    [Range(0f, 20f)]
    public float scaleSpeed;
    [Range(0f, 50f)]
    public float scaleOffset;
    [Header("Other Objects")]
    public GameObject panPrefab;
    public ScrollRect scrollRect;

    private GameObject[] _instPans;
    private Vector2[] _panPos;
    private Vector2[] _panScales;
    private RectTransform _contentRect;
    private int _selectedPanID;
    private bool isScrolling;
    private Vector2 _contentVector;

    private void Start() 
    {
        _contentRect = GetComponent<RectTransform>();
        _instPans = new GameObject[panCount];
        _panPos = new Vector2[panCount];
        _panScales = new Vector2[panCount];

        for(int i = 0; i < panCount; i++)
        {
            _instPans[i] = Instantiate(panPrefab, transform, false);
            if(i > 0)
            {
                _instPans[i].transform.localPosition = new Vector2(_instPans[i-1].transform.localPosition.x + 
                    panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, _instPans[i].transform.localPosition.y);
                
            }
            _panPos[i] = -_instPans[i].transform.localPosition;
        }
    }

    private void FixedUpdate() 
    {
        if(_contentRect.anchoredPosition.x >= _panPos[0].x && !isScrolling || _contentRect.anchoredPosition.x <= _panPos[_panPos.Length - 1].x){
            scrollRect.inertia = false;
        }
        float nearestPos = float.MaxValue;
        for(int i = 0; i < panCount; i++){
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _panPos[i].x);
            if(distance<nearestPos){
                nearestPos =distance;
                _selectedPanID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            _panScales[i].x = Mathf.SmoothStep(_instPans[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            _panScales[i].y = Mathf.SmoothStep(_instPans[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            _instPans[i].transform.localScale = _panScales[i];


        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if(scrollVelocity < 400 && !isScrolling)
        {
            scrollRect.inertia = false;
        }

        if(isScrolling || scrollVelocity > 400){
            return;
        }
        
            _contentVector.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panPos[_selectedPanID].x,
                snapSpeed * Time.fixedDeltaTime);
            _contentRect.anchoredPosition = _contentVector;
        
    }
    public void Scrolling(bool scroll){
        isScrolling = scroll;
        if(scroll){
            scrollRect.inertia = true;
        }
    }
}
