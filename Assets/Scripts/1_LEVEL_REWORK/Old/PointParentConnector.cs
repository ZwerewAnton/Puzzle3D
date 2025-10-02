using System;
using System.Collections.Generic;

namespace Gameplay
{
    [Serializable]
    public class PointParentConnector
    {
#if UNITY_EDITOR
        public List<bool> showParentDetailList = new(); 
#endif
        public Point point;
        public List<Parent> parentList = new();
        public bool IsInstalled { get; private set; }

        public void Install()
        {
            IsInstalled = true;
        }

        public void Uninstall()
        {
            IsInstalled = false;
        }
    
        public bool IsReady()
        {
            foreach (var parent in parentList)
            {
                if (!parent.IsParentInstall())
                {
                    return false;
                }
            }
            return true;
        }
    }
}