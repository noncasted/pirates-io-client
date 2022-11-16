﻿#region

using GamePlay.Player.Entity.Components.Abstract;
using GamePlay.Player.Entity.Setup.Bootstrap;
using GamePlay.Player.Entity.Setup.Flow.Callbacks;
using UnityEngine;
using VContainer;

#endregion

namespace GamePlay.Player.Entity.Weapons.Cannon.Components.Bootstrap
{
    [DisallowMultipleComponent]
    public class CanonComponentsBootstrapper : MonoBehaviour, IPlayerContainerBuilder
    {
        [SerializeField] private PlayerComponentAsset[] _assets;

        public void OnBuild(IContainerBuilder builder)
        {
            foreach (var asset in _assets)
                asset.Register(builder);
        }

        public void Resolve(IObjectResolver resolver, ICallbackRegister callbackRegister)
        {
            foreach (var asset in _assets)
                asset.Resolve(resolver, callbackRegister);
        }
    }
}