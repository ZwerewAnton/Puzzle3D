using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleDetailPoint
{
    public Vector3 Position;
    public Vector3 Rotation;
    public List<GameObject> BaseList;
    private bool _installed;
    public bool Installed
    {
        get => _installed;
        set => _installed = value;
    }
    
    [SerializeField]
    public List<SimpleDetailBase> BaseList1;
    //private List<GameObject> baseList = new List<GameObject>();
    public SimpleDetailPoint(Vector3 position, Vector3 rotation, List<GameObject> baseList) // fill the data
    {
        Position = position;
        Rotation = rotation;
        BaseList = baseList;
    }
}
