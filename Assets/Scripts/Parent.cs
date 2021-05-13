using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Parent
{
    #if UNITY_EDITOR
    public List<bool> showParentPointList = new List<bool>();  
    #endif
    
    public List<bool> checkToogleList = new List<bool>();
    public Detail parentDetail;
    public List<Point> parentPointList = new List<Point>(); 
    //public List<PointParentConnector> parentPPCList = new List<PointParentConnector>(); 

    public List<PointParentConnector> GetAllPPC()
    {
        List<PointParentConnector> pointParentConnectors = new List<PointParentConnector>();
        for(int i = 0; i < parentDetail.points.Count; i++)
        {
            if(checkToogleList[i])
            {
                pointParentConnectors.Add(parentDetail.points[i]);
            }
        }
        return pointParentConnectors;
    }

    public bool IsParentInstall()
    {
        List<PointParentConnector> list = GetAllPPC();
        foreach(PointParentConnector ppc in list){
            if(!ppc.IsInstalled)
            {
                return false;
            }
        }
        return true;
    }
}
