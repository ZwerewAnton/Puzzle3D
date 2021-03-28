using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleDetail:MonoBehaviour, IDetail
{
    private bool _installed;
    [SerializeField]
    private float _count = 1;
    public GameObject _prefab;

    public Sprite icon;
    //TODO Make a integer field in inspector
    
    [SerializeField]
    private List<SimpleDetailPoint> points;

    public bool Installed
    {
        get => _installed;
        set => _installed = value;
    }

    public List<SimpleDetailPoint> GetPoints
    {
        get => points;
    }
    
    public float Count{
        get => _count;
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

 