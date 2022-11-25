﻿using System;
using Cysharp.Threading.Tasks;
using GamePlay.Cities.Global.Registry.Runtime;
using GamePlay.Cities.Instance.Root.Runtime;
using GamePlay.Common.SceneObjects.Runtime;
using GamePlay.Factions.Selections.Loops.Runtime;
using GamePlay.Player.Entity.Components.Definition;
using GamePlay.Player.Entity.States.Deaths.Runtime;
using GamePlay.Services.LevelCameras.Runtime;
using GamePlay.Services.LevelLoops.Logs;
using GamePlay.Services.PlayerPositionProviders.Runtime;
using GamePlay.Services.PlayerSpawn.Factory.Runtime;
using GamePlay.Services.TransitionScreens.Runtime;
using GamePlay.Services.TravelOverlays.Runtime;
using Global.Services.CurrentCameras.Runtime;
using Local.Services.Abstract.Callbacks;
using UniRx;
using UnityEngine;
using VContainer;

namespace GamePlay.Services.LevelLoops.Runtime
{
    [DisallowMultipleComponent]
    public class LevelLoop :
        MonoBehaviour,
        ILocalLoadListener,
        ILevelLoop,
        ILocalSwitchListener
    {
        [Inject]
        private void Construct(
            ISceneObjectsHandler sceneObjects,
            IPlayerFactory playerFactory,
            ICurrentCamera currentCamera,
            ILevelCamera levelCamera,
            ITransitionScreen transitionScreen,
            IFactionSelectionLoop factionSelection,
            ICitiesRegistry citiesRegistry,
            ITravelOverlay travelOverlay,
            IPlayerEntityProvider playerEntityProvider,
            IPlayerEntityPresenter entityPresenter,
            LevelLoopLogger logger)
        {
            _entityPresenter = entityPresenter;
            _travelOverlay = travelOverlay;
            _citiesRegistry = citiesRegistry;
            _factionSelection = factionSelection;
            _logger = logger;
            _transitionScreen = transitionScreen;
            _sceneObjects = sceneObjects;
            _playerFactory = playerFactory;
            _currentCamera = currentCamera;
            _levelCamera = levelCamera;
        }

        private ICity _lastCity;

        private IDisposable _deathListener;
        
        private ICitiesRegistry _citiesRegistry;
        private ICurrentCamera _currentCamera;
        private ILevelCamera _levelCamera;
        
        private IFactionSelectionLoop _factionSelection;

        private LevelLoopLogger _logger;
        private IPlayerFactory _playerFactory;
        private ISceneObjectsHandler _sceneObjects;
        private ITransitionScreen _transitionScreen;
        private ITravelOverlay _travelOverlay;
        private IPlayerEntityPresenter _entityPresenter;

        public void OnEnabled()
        {
            _deathListener = MessageBroker.Default.Receive<PlayerDeathEvent>().Subscribe(OnPlayerDeath);
        }

        public void OnDisabled()
        {
            _deathListener?.Dispose();
        }

        public void OnLoaded()
        {
            _sceneObjects.InvokeFullStartup();

            _currentCamera.SetCamera(_levelCamera.Camera);

            _logger.OnLoaded();

            Begin().Forget();
        }

        private async UniTask Begin()
        {
            var selectedCity = await _factionSelection.SelectAsync();
            _transitionScreen.ToPlayerRespawn();

            var cityInstance = _citiesRegistry.GetCity(selectedCity);
            var spawnPosition = cityInstance.SpawnPoints.GetRandom();
            _lastCity = cityInstance;
            
            _logger.OnPlayerSpawn();

            var player = await _playerFactory.Create(spawnPosition, ShipType.Boat);

            _levelCamera.Teleport(player.Transform.position);
            _levelCamera.StartFollow(player.Transform);

            await _transitionScreen.FadeOut();

            _travelOverlay.Open();

            player.Respawn();
        }

        public void Respawn(ShipType ship)
        {
            ProcessRespawn(ship).Forget();
        }

        private void OnPlayerDeath(PlayerDeathEvent data)
        {
            ProcessRespawn(ShipType.Boat).Forget();
        }

        private async UniTaskVoid ProcessRespawn(ShipType ship)
        {
            _entityPresenter.DestroyPlayer();

            _logger.OnPlayerSpawn();

            var spawnPosition = _lastCity.SpawnPoints.GetRandom();
            var player = await _playerFactory.Create(spawnPosition, ship);

            _levelCamera.Teleport(player.Transform.position);
            _levelCamera.StartFollow(player.Transform);

            await _transitionScreen.FadeOut();

            player.Respawn();
        }
    }
}