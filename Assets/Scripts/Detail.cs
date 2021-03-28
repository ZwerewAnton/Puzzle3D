using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Detail : MonoBehaviour
{
    public List<bool> showPointList = new List<bool>();  
    public List<bool> showParentDetailList = new List<bool>();  
    public List<bool> showParentPointList = new List<bool>();  
    public List<bool> showParentsList = new List<bool>();  

    public float count = 1;
    public GameObject _prefab;
    public Sprite icon;

    private float _count;
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
    

    public bool IsLastDetail(){
        if(_count < 1){
            return true;
        }
        else{
            _count = _count - 1;
            return false;
        }
    }
}
