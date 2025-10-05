using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _1_LEVEL_REWORK.New.Data;
using _1_LEVEL_REWORK.New.Instances;
using Common;
using Gameplay.Spawn;
using Level;
using SaveSystem;
using Zenject;

namespace Gameplay
{
    public class LevelService : IDisposable
    {
        private readonly LevelData _levelData;
        private readonly DetailPrefabSpawner _spawner;
        private readonly LevelState _levelState;
        private readonly SaveLoadService _saveLoadService;
        private readonly int _selectedLevel;
        private CancellationTokenSource _cts;
        
        [Inject]
        private LevelService(
            DetailPrefabSpawner spawner, 
            GameState gameState, 
            LevelsRepository repository, 
            LevelState levelState, 
            SaveLoadService saveLoadService)
        {
            _spawner = spawner;
            _selectedLevel = gameState.SelectedLevel;
            _levelData = repository.GetLevel(_selectedLevel);
            _levelState = levelState;
            _saveLoadService = saveLoadService;
        }

        public async Task InitializeLevel()
        {
            _cts = new CancellationTokenSource();
            var saveData = await _saveLoadService.LoadLevelDataAsync(_selectedLevel, _cts.Token);
            _levelState.CreateDetailsInstances(_levelData.Ground, _levelData.Details, saveData.details);
            SpawnStartDetailPrefabs();
        }

        private void SpawnStartDetailPrefabs()
        {
            var installedDetails = _levelState.GetInstalledDetails();
            var spawnInfoList = new List<DetailPrefabSpawnInfo>();
            foreach (var installedDetail in installedDetails)
            {
                foreach (var pointInstance in installedDetail.Points)
                {
                    if (!pointInstance.IsInstalled) 
                        continue;
                    
                    var spawnInfo = new DetailPrefabSpawnInfo(installedDetail.GetDetailPrefab(), pointInstance.Position, pointInstance.Rotation);
                    spawnInfoList.Add(spawnInfo);
                }
            }
            _spawner.SpawnPrefabs(spawnInfoList);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}