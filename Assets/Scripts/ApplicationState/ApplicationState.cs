namespace ApplicationState
{
    public class ApplicationState
    {
        public bool IsFirstMenuLaunch { get; private set; } = true;

        public void MarkMenuAsLaunched()
        {
            IsFirstMenuLaunch = false;
        }
    }
}