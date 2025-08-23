using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelContainerData", menuName = "Data/Level", order = 51)]
    public class LevelContainerData : ScriptableObject
    {
        [FormerlySerializedAs("_levels")] [SerializeField] private List<Level> levels;
        
        public List<Level> GetLevels()
        {
            return levels;
        }
        
        public Level GetLevel(int levelID)
        {
            return levels[levelID];
        }
    }
}
