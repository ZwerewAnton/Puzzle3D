namespace _1_LEVEL_REWORK.New.Instances
{
    public class PointInstance
    {
        public bool IsInstalled { get; private set; }

        public void Install() => IsInstalled = true;
        public void Uninstall() => IsInstalled = false;
    }
}