using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Parent
{
    public List<bool> checkToogleList = new List<bool>();
    public Detail parentDetail;
    public List<Point> parentPointList = new List<Point>(); 
}
