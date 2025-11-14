namespace UI.Game.DetailsScroll
{
    public class DragOutInfo
    {
        public DetailItemModel DetailItemModel;
        public int PointerId;

        public DragOutInfo(DetailItemModel detailItemModel, int pointerId)
        {
            DetailItemModel = detailItemModel;
            PointerId = pointerId;
        }
    }
}