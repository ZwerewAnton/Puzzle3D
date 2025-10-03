using System.Collections.Generic;
using _1_LEVEL_REWORK.New.Data;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Level/LevelsRepository")]
    public class LevelsRepository : ScriptableObject
    {
        [SerializeField] private List<LevelData> levels;

        public LevelData GetLevel(int index)
        {
            return levels[index];
        }
    }
}