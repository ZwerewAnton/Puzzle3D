using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISettingsMenuItem : MonoBehaviour
    {
        [HideInInspector]
        public Image img;
        public Sprite offImg;
        public Sprite onImg;
        public RectTransform trans;
        private bool _isOn;

        private void Awake()
        {
            img = GetComponent<Image>();
            trans = (RectTransform)transform;
        }
    
        public void Click()
        {
            if (_isOn)
            {
                img.sprite = offImg;
                _isOn = false;
            }
            else
            {
                img.sprite = onImg;
                _isOn = true;
            }
        }
    }
}