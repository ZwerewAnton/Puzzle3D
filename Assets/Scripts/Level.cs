using System.Collections.Generic;
using System;

[Serializable]
public class LevelSaver
{
    public List<DetailSaver> detailList = new List<DetailSaver>();
    public float percent;
}

[Serializable]
public class PointParentConnectorSaver
{
    public bool _isInstalled;
}

[Serializable]
public class DetailSaver
{
    public string detailName;
    public int currentCount;
    public List<PointParentConnectorSaver> parentList = new List<PointParentConnectorSaver>();
}