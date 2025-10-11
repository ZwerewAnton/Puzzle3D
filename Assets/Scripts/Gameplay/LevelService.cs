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
using SaveSystem.DataObjects.Level.New;
using UI.Game.DetailsScroll;
using UI.Mediators;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class LevelService : IDisposable
    {
        private readonly DetailPrefabSpawner _spawner;
        private readonly LevelState _levelState;
        private readonly SaveLoadService _saveLoadService;
        private readonly GameState _gameState;
        private readonly LevelsRepository _repository;
        private readonly LevelMediator _levelMediator;
        private LevelData _levelData;
        private CancellationTokenSource _cts;
        
        [Inject]
        private LevelService(
            DetailPrefabSpawner spawner,
            GameState gameState,
            LevelsRepository repository,
            LevelState levelState,
            SaveLoadService saveLoadService,
            LevelMediator levelMediator)
        {
            _spawner = spawner;
            _levelState = levelState;
            _saveLoadService = saveLoadService;
            _gameState = gameState;
            _repository = repository;
            _levelMediator = levelMediator;
        }

        public async Task InitializeLevel()
        {
            _cts = new CancellationTokenSource();
        
            var result = _repository.TryGetLevel(_gameState.SelectedLevelName, out _levelData);
            if (!result)
                return;
        
            var saveData = await _saveLoadService.LoadLevelDataAsync(_levelData.LevelName, _cts.Token);
        
            InitializeLevelState(saveData);
            InitializeDetailsScrollController();
            SpawnStartDetailPrefabs();
        }

        private void InitializeLevelState(LevelSaveData saveData)
        {
            _levelState.CreateDetailsInstances(_levelData.Ground, _levelData.Details, saveData.details);
        }

        private void InitializeDetailsScrollController()
        {
            var details = _levelState.Details;
            var detailModels = new List<DetailItemModel>();
            foreach (var detailInstancePair in details)
            {
                var detailInstance = detailInstancePair.Value;
                
                if (detailInstance.IsAllInstalled())
                    continue;
                
                detailModels.Add(new DetailItemModel
                {
                    ID = detailInstancePair.Key,
                    Icon = detailInstance.GetDetailIcon(),
                    Count = detailInstance.RemainingCount
                });
                
            }
            _levelMediator.InitializeLevelScroll(detailModels);
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