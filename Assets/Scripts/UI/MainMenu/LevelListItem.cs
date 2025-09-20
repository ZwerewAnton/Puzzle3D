using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelListItem : MonoBehaviour
    {
        [SerializeField] private Image levelIconImage;
        [SerializeField] private TMP_Text levelPercent;
        [SerializeField] private TMP_Text levelName;
    
        public void SetIcon(Sprite icon)
        {
            levelIconImage.sprite = icon;
        }
        
        public void SetProgressPercent(float percent)
        {
            levelPercent.text = percent + "%";
        }
        
        public void SetLevelName(string levelName)
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