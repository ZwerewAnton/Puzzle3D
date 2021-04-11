using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    public List<DetailSaver> detailList = new List<DetailSaver>();
    public float percent;

}

[Serializable]
public class PointSaver
{
    public float[] position;
    public float[] rotation;
}

[Serializable]
public class ParentSaver
{
    public string parentName;
}

[Serializable]
public class PointParentConnectorSaver
{
    public bool _isInstalled;
    public PointSaver point;
    public List<ParentSaver> parentList = new List<ParentSaver>();
}

[Serializable]
public class DetailSaver
{
    public string detailName;
    public float _count;
    private bool _installed;
    public List<PointParentConnectorSaver> parentList = new List<PointParentConnectorSaver>();
}