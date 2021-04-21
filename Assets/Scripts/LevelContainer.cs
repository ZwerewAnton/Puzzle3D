using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelContainer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Detail> _level;    
    [SerializeField]
    private List<Level> _level1;

    public static LevelContainer currentLevelContainer;

    private void Awake() 
    {
        _level = _level1[SaveLevel.levelID].details;
        //_currentLevel = new List<Detail>(_level);
/*         foreach(Detail detail in _currentLevel)
        {
            detail.CurrentCount = detail.count;
        } */
        currentLevelContainer = this;
    }
    

    public List<Detail> Reset()
    {
        foreach(Detail detail in _level)
        {
            detail.Reset();
        }
        return new List<Detail>(_level);
    }

    public List<Detail> GetLoadLevel()
    {
        if(SaveLevel.LoadGame())
        {
            return GetCurrentLevel();
        }
        else
        {
            return Reset();
        }
    }

    public List<Detail> GetCurrentLevel()
    {
        return new List<Detail>(_level);
    }
}

[Serializable]
public class Level
{
    public float percent = 0f;
    public List<Detail> details = new List<Detail>();
}
