using System.Collections.Generic;
using Level;
using UI.MainMenu.LevelScroll;
using UI.Scroll;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelScrollRect : MonoBehaviour
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
        public LevelScrollItem panPrefab;
        public ScrollRect scrollRect;
        [FormerlySerializedAs("_contentRect")] [SerializeField] private RectTransform contentRect;
        
        private LevelScrollItem[] _instPans;
        private Vector2[] _panPos;
        private Vector3[] _panScales;
        private int _selectedPanID;
        private bool _isScrolling;
        private Vector2 _contentVector;

        public void SetItems(List<LevelItemModel> models)
        {
            _panCount = models.Count;
            _instPans = new LevelScrollItem[_panCount];
            _panPos = new Vector2[_panCount];
            _panScales = new Vector3[_panCount];

            for(var i = 0; i < _panCount; i++)
            {
                _instPans[i] = Instantiate(panPrefab, transform, false);                
                _instPans[i].SetIcon(models[i].levelIcon);
                _instPans[i].SetName(models[i].levelName);
                //_instPans[i].SetPercent(models[i].progressPercent);
                if (i > 0)
                {
                    var xPosition = _instPans[i - 1].GetLocalPosition().x + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset;
                    var yPosition = _instPans[i].GetLocalPosition().y;
                    _instPans[i].SetLocalPosition(new Vector2(xPosition, yPosition));
                }
                _panPos[i] = -_instPans[i].GetLocalPosition();
            }
        }

        private void Update() 
        {
            if (contentRect.anchoredPosition.x >= _panPos[0].x && !_isScrolling || contentRect.anchoredPosition.x <= _panPos[^1].x)
            {
                scrollRect.inertia = false;
            }
            
            var nearestPos = float.MaxValue;
            var time = Time.deltaTime;
            for (var i = 0; i < _panCount; i++)
            {
                var distance = Mathf.Abs(contentRect.anchoredPosition.x - _panPos[i].x);
                if (distance < nearestPos)
                {
                    nearestPos =distance;
                    _selectedPanID = i;
                }
                var scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
                _panScales[i].x = Mathf.SmoothStep(_instPans[i].GetLocalScale().x, scale, scaleSpeed * time);
                _panScales[i].y = Mathf.SmoothStep(_instPans[i].GetLocalScale().y, scale, scaleSpeed * time);
                _panScales[i].z = Mathf.SmoothStep(_instPans[i].GetLocalScale().z, scale, scaleSpeed * time);
                _instPans[i].SetLocalScale(_panScales[i]);
            }
            var scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
            if (scrollVelocity < 400 && !_isScrolling)
            {
                scrollRect.inertia = false;
            }

            if (_isScrolling || scrollVelocity > 400)
            {
                return;
            }
            _contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, _panPos[_selectedPanID].x, snapSpeed * time);
            contentRect.anchoredPosition = _contentVector;
        }
    
        public void Scrolling(bool scroll)
        {
            _isScrolling = scroll;
            if (scroll)
            {
                scrollRect.inertia = true;
            }
        }
    
        public int GetLevelID()
        {
            return _selectedPanID;
        }
    
        private static float GetPercent(int levelID)
        {
            // var key = PropertiesStorage.GetPercentKey() + levelID;
            // return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : 0f;
            return 0f;
        }
    
        public void UpdatePercents()
        {
            // for (var i = 0; i < _panCount; i++)
            // {
            //     _instPans[i].SetPercent(GetPercent(i));
            // }
        }
    }
}