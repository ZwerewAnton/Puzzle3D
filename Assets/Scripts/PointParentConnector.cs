using System.Collections.Generic;
using System;

[Serializable]
public class PointParentConnector
{
    #if UNITY_EDITOR
    public List<bool> showParentDetailList = new List<bool>(); 
    #endif

    private bool _isInstalled;
    public Point point;
    public List<Parent> parentList = new List<Parent>();

    public bool IsInstalled
    {
        get
        {
            return _isInstalled;
        }
    }

    public void Install()
    {
        _isInstalled = true;
    }

    public void Uninstall()
    {
        _isInstalled = false;
    }
    
    public bool IsReady()
    {
        foreach(Parent parent in parentList)
        {
            if(!parent.IsParentInstall()){
                return false;
            }
        }
        return true;
    }

}
