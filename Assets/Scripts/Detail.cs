using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public class Detail : MonoBehaviour
{
    #if UNITY_EDITOR
    public List<bool> showPointList = new List<bool>();  
    public List<bool> showParentsList = new List<bool>();  
    #endif
    
    public int count = 1;
    public GameObject _prefab;
    public Sprite icon;
    public bool isRoot;

    private int _currentCount = 0;
    private bool _installed;

    public int CurrentCount
    {
        get
        {
            return _currentCount;
        }
        set
        {
            _currentCount = value;
        }
    }

    public List<PointParentConnector> points = new List<PointParentConnector>();

    public bool Installed
    {
        get => _installed;
        set => _installed = value;
    }

    public List<PointParentConnector> GetPoints
    {
        get => points;
    }
    public List<Point> GetAvaiablePoints()
    {
        List<Point> list = new List<Point>();
        foreach(PointParentConnector pointParentConnector in points)
        {
            if(pointParentConnector.IsReady() && !pointParentConnector.IsInstalled)
            {
                list.Add(pointParentConnector.point);
            }
        }
        return list;

    }
    public void Reset()
    {
        _currentCount = count;
        foreach(PointParentConnector pointParConn in points)
        {
            pointParConn.Uninstall();
        }
    }

    public bool IsLastDetail()
    {
        if(_currentCount == 0)
        {
            _currentCount = count;
        }
        if(_currentCount == 1)
        {
            _currentCount = _currentCount - 1;
            return true;
        }
        else
        {
            _currentCount = _currentCount - 1;
            return false;
        }
    }
}
