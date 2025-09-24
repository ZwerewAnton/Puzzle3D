namespace Common
{
    public class GameState
    {
        public bool IsFirstMenuLaunch { get; private set; } = true;
        public int SelectedLevel { get; private set; }

        public void MarkMenuAsLaunched()
        {
            IsFirstMenuLaunch = false;
        }    
        
        public void SelectLevel(int levelIndex)
        {
            SelectedLevel = levelIndex;
        }
    }
}