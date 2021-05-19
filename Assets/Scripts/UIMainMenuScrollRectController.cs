using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuScrollRectController : MonoBehaviour
{
    private int _panCount;
    [Range(0, 500)]
    public int panOffset;
    [Range(0f, 20f)]
    public float snapSpeed;
    [Range(0f, 20f)]
    public float scaleSpeed;
    [Range(0f, 50f)]
    public float scaleOffset;
    [Header("Other Objects")]
    public UIMainMenuListItem panPrefab;
    public ScrollRect scrollRect;

    private UIMainMenuListItem[] _instPans;
    private Vector2[] _panPos;
    private Vector3[] _panScales;
    [SerializeField] private RectTransform _contentRect;
    private int _selectedPanID;
    private bool isScrolling;
    private Vector2 _contentVector;

    private void Start() 
    {
        _panCount = LevelContainer.currentLevelContainer.GetLevelCount();
        Sprite[] sprites = LevelContainer.currentLevelContainer.GetLevelIcons();
        string[] names = LevelContainer.currentLevelContainer.GetLevelNames();
        _instPans = new UIMainMenuListItem[_panCount];
        _panPos = new Vector2[_panCount];
        _panScales = new Vector3[_panCount];

        for(int i = 0; i < _panCount; i++)
        {
            _instPans[i] = Instantiate(panPrefab, transform, false);                
            _instPans[i].SetIcon(sprites[i]);
            _instPans[i].SetName(names[i]);
            _instPans[i].SetPercent(GetPercent(i));
            if(i > 0)
            {
                _instPans[i].SetLocalPosition(new Vector2(_instPans[i-1].GetLocalPosition().x + 
                    panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, _instPans[i].GetLocalPosition().y));

                
            }
            
            //_panPos[i] = -_instPans[i].transform.localPosition;
            _panPos[i] = -_instPans[i].GetLocalPosition();
        }
    }

    private void FixedUpdate() 
    {
        if(_contentRect.anchoredPosition.x >= _panPos[0].x && !isScrolling || _contentRect.anchoredPosition.x <= _panPos[_panPos.Length - 1].x){
            scrollRect.inertia = false;
        }
        float nearestPos = float.MaxValue;
        for(int i = 0; i < _panCount; i++){
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _panPos[i].x);
            if(distance<nearestPos){
                nearestPos =distance;
                _selectedPanID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            _panScales[i].x = Mathf.SmoothStep(_instPans[i].GetLocalScale().x, scale, scaleSpeed * Time.fixedDeltaTime);
            _panScales[i].y = Mathf.SmoothStep(_instPans[i].GetLocalScale().y, scale, scaleSpeed * Time.fixedDeltaTime);
            _panScales[i].z = Mathf.SmoothStep(_instPans[i].GetLocalScale().z, scale, scaleSpeed * Time.fixedDeltaTime);
            _instPans[i].SetLocalScale(_panScales[i]);
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
    public int GetLevelID(){
        return _selectedPanID;
    }
    private float GetPercent(int levelID)
    {
        string key = PropertiesStorage.GetPercentKey() + levelID.ToString();
        if(PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return 0f;
        }
    }
    public void UpdapePercents()
    {
        for(int i = 0; i < _panCount; i++)
        {
            _instPans[i].SetPercent(GetPercent(i));
        }
    }
}
