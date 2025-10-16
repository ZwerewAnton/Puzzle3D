using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _1_LEVEL_REWORK.New.Data;
using _1_LEVEL_REWORK.New.Instances;
using Common;
using Gameplay.Movement;
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
        private readonly DetailViewMover _detailViewMover;
        private LevelData _levelData;
        private CancellationTokenSource _cts;
        
        [Inject]
        private LevelService(
            DetailPrefabSpawner spawner,
            GameState gameState,
            LevelsRepository repository,
            LevelState levelState,
            SaveLoadService saveLoadService,
            LevelMediator levelMediator,
            DetailViewMover detailViewMover)
        {
            _spawner = spawner;
            _levelState = levelState;
            _saveLoadService = saveLoadService;
            _gameState = gameState;
            _repository = repository;
            _levelMediator = levelMediator;
            _detailViewMover = detailViewMover;
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
            _detailViewMover.PlacementEnded += OnDetailViewPlacementEnded;
        }

        private void InitializeLevelState(LevelSaveData saveData)
        {
            _levelState.CreateDetailsInstances(_levelData.Ground, _levelData.Details, saveData.details);
        }

        private void InitializeDetailsScrollController()
        {
            var details = _levelState.Details;
            var detailModels = new List<DetailItemModel>();
            foreach (var (id, detailInstance) in details)
            {
                if (detailInstance.IsAllInstalled())
                    continue;
                
                detailModels.Add(new DetailItemModel
                {
                    ID = id,
                    Icon = detailInstance.GetDetailIcon(),
                    Count = detailInstance.RemainingCount
                });
                
            }
            _levelMediator.InitializeLevelScroll(detailModels);
            _levelMediator.DetailItemDragStarted += OnDetailItemDragStarted;
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
        
        private void OnDetailItemDragStarted(DetailItemModel detailItemModel)
        {
            var detailInstance = _levelState.Details[detailItemModel.ID];
            StartDetailViewMove(detailInstance);
        }

        private void StartDetailViewMove(DetailInstance detailInstance)
        {
            var pointList = detailInstance.Points.Select(pointInstance => new PointTransform(pointInstance.Position, pointInstance.Rotation)).ToList();
            
            _detailViewMover.StartMove(detailInstance.GetDetailMesh(), detailInstance.GetDetailMaterial(), pointList);
        }

        private void OnDetailViewPlacementEnded(DetailPlacementResult placementResult)
        {
            Debug.Log(placementResult.Success + " " + placementResult.PointIndex);
        }
        
        public void Dispose()
        {
            _detailViewMover.PlacementEnded -= OnDetailViewPlacementEnded;
            _levelMediator.DetailItemDragStarted -= OnDetailItemDragStarted;
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}