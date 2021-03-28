using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleDetailBase
{
    public SimpleDetail baseDetail;
    public List<DetailBasePoint> basePointList; 
}

[Serializable]
public class DetailBasePoint
{
    public Vector3 position;
    public Vector3 rotation;
}