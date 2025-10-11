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
    }
}