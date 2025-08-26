using System.IO;
using UnityEngine;

namespace Utils.Paths
{
    public static class Paths
    {
        public static readonly string PathToSaveData = Path.Combine(Application.persistentDataPath, "save.data");
        public static readonly string PathToSaveDirectory = Path.Combine(Application.persistentDataPath, "saves");
        
        public static string GetPathToProgressData()
        {
            return Path.Combine(PathToSaveDirectory, "progress.data");
        }
        
        public static string GetPathToLevelDataDirectory(int levelId)
        {
            return Path.Combine(PathToSaveDirectory, levelId.ToString());
        }
        
        public static string GetPathToLevelData(int levelId)
        {
            return Path.Combine(PathToSaveDirectory, levelId.ToString(), "save.data");
        }
        
    }
}