using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gameplay;
using Level;
using SaveSystem.DataObjects.Level;
using SaveSystem.DataObjects.Progress;
using UnityEngine;
using Utils.Paths;

namespace SaveSystem
{
    public class SaveLoadService
    {
        private ProgressSaveData _progressData = new();
        
        public async Task<bool> LoadProgressDataAsync()
        {
            if (!Directory.Exists(Paths.PathToSaveDirectory) ||
                !File.Exists(Paths.GetPathToProgressData()))
                return false;

            _progressData = await LoadDataAsync<ProgressSaveData>(Paths.GetPathToProgressData());
            return true;
        }

        public async Task SaveProgressDataAsync(int levelId, int progress)
        {
            UpdateProgressData(levelId, progress);
            await SaveDataAsync(_progressData, Paths.PathToSaveDirectory, Paths.GetPathToProgressData());
        }
        
        public async Task<LevelSaveData> LoadLevelDataAsync(int levelId)
        {
            if (!Directory.Exists(Paths.GetPathToLevelDataDirectory(levelId)) ||
                !File.Exists(Paths.GetPathToLevelData(levelId)))
            {
                Debug.LogWarning($"Level {levelId} data not found. Returning default.");
                return new LevelSaveData();
            }
            
            return await LoadDataAsync<LevelSaveData>(Paths.GetPathToLevelData(levelId));
        }
        
        public async Task SaveLevelData(int levelId, List<Detail> allDetails)
        {
            var data = CreateLevelSaveData(allDetails);
            await SaveDataAsync(data, Paths.GetPathToLevelDataDirectory(levelId), Paths.GetPathToLevelData(levelId));
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

        private async Task<T> LoadDataAsync<T>(string path) where T : new()
        {
            try
            {
                if (!File.Exists(path))
                    return new T();

                var dataStr = await File.ReadAllTextAsync(path);
                var result = JsonUtility.FromJson<T>(dataStr);
                return result ?? new T();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Load failed: {ex.Message}");
                return new T();
            }
        }

        private async Task SaveDataAsync<T>(T dataObject, string directoryPath, string filePath)
        {
            try
            {
                var jsonStr = JsonUtility.ToJson(dataObject);
                Directory.CreateDirectory(directoryPath);
                await File.WriteAllTextAsync(filePath, jsonStr);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Save failed: {ex.Message}");
                throw;
            }
        }
    }
}