using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Parent
{
   #if UNITY_EDITOR
    public List<bool> checkToogleList = new List<bool>();
    public List<bool> showParentPointList = new List<bool>();  
    #endif
    
    public Detail parentDetail;
    public List<Point> parentPointList = new List<Point>(); 
    public List<PointParentConnector> parentPPCList = new List<PointParentConnector>(); 
}
