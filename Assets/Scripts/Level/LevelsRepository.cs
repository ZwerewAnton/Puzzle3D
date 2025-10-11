using System.Collections.Generic;
using _1_LEVEL_REWORK.New.Data;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Level/LevelsRepository")]
    public class LevelsRepository : ScriptableObject
    {
        [SerializeField] private List<LevelData> levels;

        public List<LevelData> Levels => levels;

        public LevelData GetLevel(int index)
        {
            return levels[index];
        }
        
        public bool TryGetLevel(string levelName, out LevelData level)
        {
            level = null;

            if (string.IsNullOrWhiteSpace(levelName))
            {
                Debug.LogWarning("TryGetLevel called with empty or null level name.");
                return false;
            }

            level = levels.Find(data => data.LevelName == levelName);
            if (level == null)
            {
                Debug.LogWarning($"Level not found: {levelName}");
                return false;
            }

            return true;
        }
    }
}