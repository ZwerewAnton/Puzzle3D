using TMPro;
using UI.Scroll;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.DetailsScroll
{
    public class DetailItemView: ScrollItemView<DetailItemModel>
    {
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Image detailImage;
        
        private int _showedCount;

        public override void SetData(int itemIndex, DetailItemModel model)
        {
            base.SetData(itemIndex, model);

            SetCount(model.Count);
            detailImage.sprite = model.Icon;
            detailImage.preserveAspect = true;
        }

        public void DecrementCount()
        {
            if (_showedCount > 0)
            {
                SetCount(_showedCount - 1);
            }
            else
            {
                countText.enabled = false;
                detailImage.enabled = false;
            }
        }

        private void SetCount(int count)
        {
            _showedCount = count;
            countText.SetText(_showedCount.ToString());
        }
    }
}