using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContainer", menuName = "Data/Level", order = 51)]
public class LevelContainerSO : ScriptableObject
{
    public int count;
    [SerializeField]
    private List<Level> _levels;
    private Level _level;

    public int GetLevelCount()
    {
        return _levels.Count;
    }
    

    public List<Detail> Reset()
    {
        foreach(Detail detail in _level.Details)
        {
            detail.Reset();
        }
        return new List<Detail>(_level.Details);
    }

    public List<Detail> GetLoadLevel()
    {
        if(SaveLevel.LoadGame())
        {
            return GetCurrentLevelDetails();
        }
        else
        {
            return Reset();
        }
    }

    public List<Detail> GetCurrentLevelDetails()
    {
        _level = _levels[SaveLevel.levelID];
        return new List<Detail>(_level.Details);
    }    
    public Detail GetCurrentLevelGround()
    {
        return _level.Ground;
    }
    public float GetPercent()
    {
        return _level.GetPercent();
    }
    public Sprite[] GetLevelIcons()
    {
        Sprite[] icons = new Sprite[_levels.Count];
        for(int i = 0; i < _levels.Count; i++)
        {
            icons[i] = _levels[i].Icon;
        }
        return icons;
    }
    public string[] GetLevelNames()
    {
        string[] names = new string[_levels.Count];
        for(int i = 0; i < _levels.Count; i++)
        {
            names[i] = _levels[i].Name;
        }
        return names;
    }
}
