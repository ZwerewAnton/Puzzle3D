using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Gameplay;
using Level;
using SaveSystem.DataObjects;
using SaveSystem.DataObjects.Level;
using Settings;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public static class LevelSaver
    {
        private static string _folderPath = Path.Combine(Application.persistentDataPath, "saves");
        public static int levelID;

        private static LevelSaveData CreateSaveObject(List <Detail> allDetails)
        {
            var level = new LevelSaveData();
            float installedDetailsCount = 0;
            foreach (var detail in allDetails)
            {
                var isInstalled = true;
                var detailSav = new DetailSaveData
                {
                    detailName = detail.name,
                    currentCount = detail.CurrentCount
                };

                var pPCSaverList = new List<PointParentConnectorSaveData>();
                detailSav.parentList = pPCSaverList;

                foreach (var pointPC in detail.points)
                {
                    var pPCSaveData = new PointParentConnectorSaveData
                    {
                        isInstalled = pointPC.IsInstalled
                    };

                    if (!pointPC.IsInstalled || !detail.isRoot)
                    {
                        isInstalled = false;
                    }

                    pPCSaverList.Add(pPCSaveData);
                }
                if (isInstalled)
                {
                    installedDetailsCount++;
                }
                level.details.Add(detailSav);
            }
            level.percent = Mathf.Round((installedDetailsCount/allDetails.Count) * 100);
            return level;
        }

        public static void SaveGame()
        {
            var level = CreateSaveObject(LevelContainer.currentLevelContainer.GetCurrentLevelDetails());
            var bf = new BinaryFormatter();
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            var file = File.Create(GetSavePath());
            bf.Serialize(file, level);
            file.Close();
            SavePercent(LevelContainer.currentLevelContainer.GetPercent());
        }   

        public static void SaveLevelData()
        {
            var level = CreateSaveObject(LevelContainer.currentLevelContainer.GetCurrentLevelDetails());
            var bf = new BinaryFormatter();
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            var file = File.Create(GetSavePath());
            bf.Serialize(file, level);
            file.Close();
            SavePercent(LevelContainer.currentLevelContainer.GetPercent());
        }

        public static bool LoadGame()
        {
            if (!Directory.Exists(_folderPath) || !File.Exists(GetSavePath())) 
                return false;
            
            var loadDetailList  = LevelContainer.currentLevelContainer.GetCurrentLevelDetails();
            var bf = new BinaryFormatter();
            var file = File.Open(GetSavePath(), FileMode.Open);
            var save = (LevelSaveData)bf.Deserialize(file);
            file.Close();

            foreach (var detail in save.details)
            {
                foreach (var detailContainer in loadDetailList)
                {
                    if (detailContainer.name != detail.detailName) 
                        continue;
                        
                    detailContainer.CurrentCount = detail.currentCount;
                    for (var i = 0; i < detailContainer.points.Count; i++)
                    {
                        if (detail.parentList[i].isInstalled)
                        {
                            detailContainer.points[i].Install();
                        }
                    }
                }
            }
            return true;
        }

        public static void DeleteSaveFile()
        {
            if (Directory.Exists(_folderPath) && File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
            }
        }

        private static string GetSavePath()
        {
            var saveFile = levelID + "_gamesave.save";
            return Path.Combine(_folderPath + saveFile);
        }

        private static void SavePercent(float percent)
        {
            var key = PropertiesStorage.GetPercentKey() + levelID;
            PlayerPrefs.SetFloat(key, percent);
            PlayerPrefs.Save();
        }
        
        public static float[] LoadPercents()
        {
            var levelCount = LevelContainer.currentLevelContainer.GetLevelCount();
            var percents = new float[levelCount - 1];
            for (var i = 0; i < levelCount; i++)
            {
                var key = PropertiesStorage.GetPercentKey() + i;
                if (PlayerPrefs.HasKey(key))
                {
                    percents[i] = PlayerPrefs.GetInt(key);
                }
                else
                {
                    PlayerPrefs.SetFloat(key, 0f);
                    PlayerPrefs.Save();
                }
            }
            return percents;
        }
    }
}