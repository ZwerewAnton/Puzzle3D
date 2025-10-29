using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Dto;
using SaveSystem;
using SaveSystem.DataObjects.Level.New;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class LevelSaverMono : MonoBehaviour
    {
        private SaveLoadService _saveLoadService;

        [Inject]
        private void Construct(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void SaveProgress(string levelName, int progress, List<DetailInstanceDto> details)
        {
            var data = CreateLevelSaveData(details);
            _ = SaveLevelDataAsync(levelName, progress, data);
        }

        private async Task SaveLevelDataAsync(string levelName, int progress, LevelSaveData data)
        {
            try
            {
                await _saveLoadService.SaveLevelDataAsync(levelName, data);
                await _saveLoadService.SaveProgressDataAsync(levelName, progress);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save level progress: {ex}");
            }
        }

        private LevelSaveData CreateLevelSaveData(List<DetailInstanceDto> details)
        {
            var levelSaveData = new LevelSaveData();

            foreach (var detail in details)
            {
                var detailData = new DetailSaveData
                {
                    id = detail.Id,
                    currentCount = detail.CurrentCount
                };

                foreach (var point in detail.Points)
                {
                    var pointData = new PointSaveData
                    {
                        isInstalled = point.IsInstalled
                    };
                    detailData.points.Add(pointData);
                }
            }

            return levelSaveData;
        }
    }
}