using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelScroller : MonoBehaviour
    {
        [SerializeField, Range(0, 500)] private int panOffset;
        [SerializeField, Range(0f, 20f)] private float snapSpeed;
        [SerializeField, Range(0f, 20f)] private float scaleSpeed;
        [SerializeField, Range(0f, 50f)] private float scaleOffset;
        [SerializeField] private LevelScrollItem panPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform contentRect;
        
        private LevelScrollItem[] _instPans;
        private Vector2[] _panPos;
        private Vector3[] _panScales;
        private int _selectedPanID;
        private bool _isScrolling;
        private Vector2 _contentVector;
        private int _panCount;
        
        private void Start() 
        {
            // _panCount = sLevelContainer.currentLevelContainer.GetLevelCount();
            // var sprites = sLevelContainer.currentLevelContainer.GetLevelIcons();
            // var names = sLevelContainer.currentLevelContainer.GetLevelNames();
            //
            //
            // _instPans = new UIMainMenuListItem[_panCount];
            // _panPos = new Vector2[_panCount];
            // _panScales = new Vector3[_panCount];
            //
            // for(var i = 0; i < _panCount; i++)
            // {
            //     _instPans[i] = Instantiate(panPrefab, transform, false);                
            //     _instPans[i].SetIcon(sprites[i]);
            //     _instPans[i].SetName(names[i]);
            //     _instPans[i].SetPercent(GetPercent(i));
            //     if (i > 0)
            //     {
            //         var xPosition = _instPans[i - 1].GetLocalPosition().x + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset;
            //         var yPosition = _instPans[i].GetLocalPosition().y;
            //         _instPans[i].SetLocalPosition(new Vector2(xPosition, yPosition));
            //     }
            //     _panPos[i] = -_instPans[i].GetLocalPosition();
            // }
        }

        public void InitList()
        {
            
        }
    }
}