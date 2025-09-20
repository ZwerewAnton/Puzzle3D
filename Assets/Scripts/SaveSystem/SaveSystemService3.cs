using System.Collections.Generic;
using System.IO;
using Gameplay;
using Level;
using SaveSystem.DataObjects.Level;
using SaveSystem.DataObjects.Progress;
using UnityEngine;
using Utils.Paths;

namespace SaveSystem
{
    public class SaveSystemService3
    {
        //private List<ProgressLevelSaveData> _progressData = new();
        private ProgressSaveData _progressData = new();
        public static int levelID;

        // public void SaveGame(int levelId, List<Detail> allDetails)
        // {
        //     SaveLevel(levelId, allDetails);
        //     SaveProgressData(levelId);
        // }   
        
        public bool LoadProgressData()
        {
            if (!Directory.Exists(Paths.PathToSaveDirectory) ||
                !File.Exists(Paths.GetPathToProgressData()))
                return false;

            _progressData = LoadData<ProgressSaveData>(Paths.GetPathToProgressData());
            return true;
        }

        public void SaveProgressData(int levelId, int progress)
        {
            UpdateProgressData(levelId, progress);

            var jsonStr = JsonUtility.ToJson(_progressData);
            Directory.CreateDirectory(Paths.PathToSaveDirectory);
            File.WriteAllText(Paths.GetPathToProgressData(), jsonStr);
        }

        public bool LoadLevelData(int levelId)
        {
            if (!Directory.Exists(Paths.GetPathToLevelDataDirectory(levelId)) ||
                !File.Exists(Paths.GetPathToLevelData(levelId)))
                return false;

            var loadDetailList = LevelContainer.currentLevelContainer.GetCurrentLevelDetails();
            var save = LoadData<LevelSaveData>(Paths.GetPathToLevelData(levelId));

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
        
        public void SaveLevelData(int levelId, List<Detail> allDetails)
        {
            var data = CreateLevelSaveData(allDetails);
            SaveData(data, Paths.GetPathToLevelDataDirectory(levelId), Paths.GetPathToLevelData(levelId));
        }

        private LevelSaveData CreateLevelSaveData(List<Detail> allDetails)
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

            level.percent = Mathf.Round((installedDetailsCount / allDetails.Count) * 100);
            return level;
        }

        private T LoadData<T>(string path) where T : new()
        {
            if (!File.Exists(path))
                return new T();

            var dataStr = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(dataStr);
        }

        private void SaveData<T>(T dataObject, string directoryPath, string filePath)
        {
            var jsonStr = JsonUtility.ToJson(dataObject);
            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(filePath, jsonStr);
        }

        private void UpdateProgressData(int levelId, int progress)
        {
            var dataList = _progressData.progressLevelsSaveData;
            var index = dataList.FindIndex(data => data.levelId == levelId);
            if (index == -1)
            {
                dataList.Add(new ProgressLevelSaveData(levelId, progress));
            }
            else
            {
                dataList[index].progress = progress;
            }
        }
    }
}