using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Detail> level;    
    public List<Detail> _currentLevel;

    private void Start() 
    {
        if(_currentLevel == null)
        {
            _currentLevel = new List<Detail>(level);
        }
    }

    public List<Detail> Reset()
    {
            _currentLevel = new List<Detail>(level);
            foreach(Detail detail in _currentLevel)
            {
                detail.Reset();
            }
            return _currentLevel;
        
    }
}
