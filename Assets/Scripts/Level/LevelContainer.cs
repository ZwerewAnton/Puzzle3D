using System.Collections.Generic;
using Gameplay;
using SaveSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    public class LevelContainer : MonoBehaviour
    {
        [FormerlySerializedAs("_levels")] [SerializeField] private List<Level> levels;
        [SerializeField] private LevelContainerData levelsData;
        public static LevelContainer currentLevelContainer;
        
        private Level _level;

        private void Awake() 
        {
            if (!currentLevelContainer) 
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
            return levelsData.GetLevels().Count;
        }
        
        public void ResetLevel(int levelID)
        {
            levelsData.GetLevel(levelID).Reset();
            LevelSaver.SaveGame();
        }
    
        public List<Detail> ResetAllDetails()
        {
            foreach (var detail in _level.Details)
            {
                detail.Reset();
            }
            return new List<Detail>(_level.Details);
        }

        public List<Detail> GetLoadLevel()
        {
            _level = levelsData.GetLevel(LevelSaver.levelID);
            return LevelSaver.LoadGame() ? GetCurrentLevelDetails() : ResetAllDetails();
        }

        public List<Detail> GetCurrentLevelDetails()
        {
            _level = levelsData.GetLevel(LevelSaver.levelID);
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
            var levels = levelsData.GetLevels();
            var icons = new Sprite[levels.Count];
            for (var i = 0; i < levels.Count; i++)
            {
                icons[i] = levels[i].Icon;
            }
            return icons;
        }
        
        public string[] GetLevelNames()
        {
            var levels = levelsData.GetLevels();
            var names = new string[levels.Count];
            for (var i = 0; i < levels.Count; i++)
            {
                names[i] = levels[i].Name;
            }
            return names;
        }
    }
}