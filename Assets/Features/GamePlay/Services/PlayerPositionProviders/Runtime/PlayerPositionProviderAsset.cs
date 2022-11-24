﻿using Cysharp.Threading.Tasks;
using GamePlay.Common.Paths;
using Global.Services.ScenesFlow.Runtime.Abstract;
using Local.Services.Abstract;
using UnityEngine;

namespace GamePlay.Services.PlayerPositionProviders.Runtime
{
    [CreateAssetMenu(fileName = GamePlayAssetsPaths.ServicePrefix + "PlayerPositionProvider",
        menuName = GamePlayAssetsPaths.PlayerPositionProvider + "Service")]
    public class PlayerPositionProviderAsset : LocalServiceAsset
    {
        [SerializeField] private PlayerEntityProvider _prefab;

        public override async UniTask Create(
            IServiceBinder serviceBinder,
            ICallbacksRegister callbacksRegister,
            ISceneLoader sceneLoader)
        {
            var provider = Instantiate(_prefab);
            provider.name = "PlayerPositionProvider";

            serviceBinder.RegisterComponent(provider)
                .As<IPlayerEntityPresenter>()
                .As<IPlayerEntityProvider>();

            serviceBinder.AddToModules(provider);
        }
    }
}