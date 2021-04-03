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
    public float count = 1;
    public GameObject _prefab;
    public Sprite icon;

    private float _count = 0;
    private bool _installed;

    private void Awake()
    {
        _count = count;
    }
    //TODO Make a integer field in inspector


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

    public void Reset(){
        _count = count;
        foreach(PointParentConnector pointParConn in points)
        {
            pointParConn.Uninstall();
        }
    }
    

    public bool IsLastDetail(){
        if(_count == 0)
        {
            _count = count;
        }
        if(_count == 1)
        {
            _count = _count - 1;
            return true;
        }
        else
        {
            _count = _count - 1;
            return false;
        }
    }
}
