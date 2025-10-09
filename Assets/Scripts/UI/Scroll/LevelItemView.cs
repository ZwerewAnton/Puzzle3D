using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scroll
{
    public class LevelItemView: ScrollItemView<LevelItemModel>
    {
        [SerializeField] private Image levelIcon;
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private TMP_Text progressPercent;
        
        public override void SetData(int itemIndex, LevelItemModel model)
        {
            base.SetData(itemIndex, model);
            
            levelIcon.sprite = model.levelIcon;
            levelName.SetText(model.levelName);
            progressPercent.SetText(model.progressPercent);
        }
    }
}