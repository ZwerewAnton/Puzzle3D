using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelContainer : MonoBehaviour
{
    [SerializeField]
    private List<Level> _levels;
    private Level _level;    

    public static LevelContainer currentLevelContainer;

    private void Awake() 
    {
        if (currentLevelContainer == null) 
        {
            currentLevelContainer = this;
            DontDestroyOnLoad(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    public int GetLevelCount()
    {
        return _levels.Count;
    }
    public void ResetLevel(int levelID)
    {
        _levels[levelID].Reset();
        SaveLevel.SaveGame();
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
        _level = _levels[SaveLevel.levelID];
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

[Serializable]
public class Level
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Detail _ground;
    [SerializeField] private List<Detail> _details = new List<Detail>();
    private float _percent = 0;

    public List<Detail> Details
    {
        get => _details;
    }    
    public Detail Ground
    {
        get => _ground;
    }    
    public Sprite Icon
    {
        get => _icon;
    }    
    public string Name
    {
        get => _name;
    }
    public void Reset()
    {
        foreach(Detail detail in _details)
        {
            detail.Reset();
        }
    }

    public float GetPercent()
    {
        float allDetails = 0;
        float installedDetails = 0;
        foreach(Detail detail in _details)
        {
            foreach(PointParentConnector pPC in detail.GetPoints)
            {
                if(pPC.IsInstalled){
                    installedDetails++;
                }
                allDetails++;
            }
        }
        return Mathf.Round((installedDetails/allDetails) * 100);
    }
}
