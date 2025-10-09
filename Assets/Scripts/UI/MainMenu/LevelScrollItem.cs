using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class LevelScrollItem : MonoBehaviour
    {
        [FormerlySerializedAs("_iconImage")] [SerializeField] private Image levelIconImage;
        [FormerlySerializedAs("_percent")] [SerializeField] private TMP_Text levelPercent;
        [FormerlySerializedAs("_name")] [SerializeField] private TMP_Text levelName;
    
        public void SetIcon(Sprite icon)
        {
            levelIconImage.sprite = icon;
        }
        
        public void SetPercent(float percent)
        {
            levelPercent.text = percent + "%";
        }
        
        public void SetName(string levelName)
        {
            this.levelName.text = levelName;
        }
        
        public void SetLocalScale(Vector3 scale)
        {
            gameObject.transform.localScale = scale;
        }
        
        public void SetLocalPosition(Vector3 position)
        {
            gameObject.transform.localPosition = position;
        }
        
        public Vector3 GetLocalPosition()
        {
            return transform.localPosition;
        }
        
        public Vector3 GetLocalScale()
        {
            return transform.localScale;
        }
    }
}