using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gameplay;
using SaveSystem.DataObjects.Level;
using SaveSystem.DataObjects.Level.New;
using SaveSystem.DataObjects.Progress;
using UnityEngine;
using Utils.Paths;

namespace SaveSystem
{
    public class SaveLoadService
    {
        private ProgressSaveData _progressData = new();
        
        public async Task LoadProgressDataAsync(CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(Paths.PathToSaveDirectory) ||
                !File.Exists(Paths.GetPathToProgressData()))
                return;

            _progressData = await LoadDataAsync<ProgressSaveData>(Paths.GetPathToProgressData(), cancellationToken);
        }

        public async Task SaveProgressDataAsync(int levelId, int progress)
        {
            UpdateProgressData(levelId, progress);
            await SaveDataAsync(_progressData, Paths.PathToSaveDirectory, Paths.GetPathToProgressData());
        }
        
        public async Task<LevelSaveData> LoadLevelDataAsync(int levelId, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(Paths.GetPathToLevelDataDirectory(levelId)) ||
                !File.Exists(Paths.GetPathToLevelData(levelId)))
            {
                Debug.Log($"Level {levelId} data not found. Returning default.");
                return new LevelSaveData();
            }
            
            return await LoadDataAsync<LevelSaveData>(Paths.GetPathToLevelData(levelId), cancellationToken);
        }
        
        public async Task SaveLevelData(int levelId, List<Detail> allDetails)
        {
            var data = CreateLevelSaveData(allDetails);
            await SaveDataAsync(data, Paths.GetPathToLevelDataDirectory(levelId), Paths.GetPathToLevelData(levelId));
        }

        private LevelSaveData_old CreateLevelSaveData(List<Detail> allDetails)
        {
            var level = new LevelSaveData_old();
            float installedDetailsCount = 0;
            foreach (var detail in allDetails)
            {
                var isInstalled = true;
                var detailSav = new DetailSaveData_old
                {
                    detailName = detail.name,
                    currentCount = detail.CurrentCount
                };

                var pPCSaverList = new List<PointParentConnectorSaveData_old>();
                detailSav.parentList = pPCSaverList;

                foreach (var pointPC in detail.points)
                {
                    var pPCSaveData = new PointParentConnectorSaveData_old
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

        private static async Task<T> LoadDataAsync<T>(string path, CancellationToken cancellationToken = default) where T : new()
        {
            try
            {
                if (!File.Exists(path))
                    return new T();

                var dataStr = await File.ReadAllTextAsync(path, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = JsonUtility.FromJson<T>(dataStr);
                return result ?? new T();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning($"LoadDataAsync cancelled: {path}");
                return new T();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Load failed: {ex.Message}");
                return new T();
            }
        }

        private static async Task SaveDataAsync<T>(T dataObject, string directoryPath, string filePath)
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