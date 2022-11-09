﻿using Common.EditableScriptableObjects.Attributes;
using Cysharp.Threading.Tasks;
using GamePlay.Common.Paths;
using GamePlay.Services.Projectiles.Factory;
using GamePlay.Services.Projectiles.Logs;
using GamePlay.Services.Projectiles.Mover;
using Global.Services.ScenesFlow.Handling.Data;
using Global.Services.ScenesFlow.Runtime.Abstract;
using Local.Services.Abstract;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;

namespace GamePlay.Services.Projectiles.Bootstrap
{
    [CreateAssetMenu(fileName = GamePlayAssetsPaths.ServicePrefix + "Projectiles",
        menuName = GamePlayAssetsPaths.Projectiles + "Service")]
    public class ProjectilesAsset : LocalServiceAsset
    {
        [SerializeField] private AssetReference _poolScene;
        [SerializeField] private ProjectilesBootstrapper _prefab;
        [SerializeField] [EditableObject] private ProjectilesMoverConfigAsset _moverConfig;
        [SerializeField] [EditableObject] private ProjectilesLogSettings _logSettings;

        public override async UniTask Create(IServiceBinder serviceBinder, ICallbacksRegister callbacksRegister,
            ISceneLoader sceneLoader)
        {
            var pool = Instantiate(_prefab);
            pool.name = "Pool_Projectiles";

            serviceBinder.Register<ProjectilesPoolProvider>()
                .As<IProjectilesPoolProvider>()
                .WithParameter("poolProvider", pool.Handler);

            serviceBinder.Register<ProjectilesLogger>()
                .WithParameter("settings", _logSettings)
                .AsSelf();

            serviceBinder.Register<ProjectilesMover>()
                .WithParameter("config", _moverConfig)
                .As<IProjectilesMover>()
                .AsSelf();

            var scene = await sceneLoader.Load(new EmptySceneLoadData(_poolScene));
            SceneManager.MoveGameObjectToScene(pool.gameObject, scene.Instance.Scene);

            callbacksRegister.ListenFlowCallbacks(pool);
            callbacksRegister.ListenContainerCallbacks(pool);

            pool.OnSceneLoaded(scene.Instance.Scene);
        }

        public override void OnResolve(IObjectResolver resolver, ICallbacksRegister callbacksRegister)
        {
            var mover = resolver.Resolve<ProjectilesMover>();
            callbacksRegister.ListenFlowCallbacks(mover);
        }
    }
}