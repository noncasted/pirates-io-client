﻿using System;
using System.Collections.Generic;
using Global.Services.ScenesFlow.Handling.Result;
using Local.Services.Abstract.Callbacks;
using VContainer.Unity;

namespace Local.ComposedSceneConfig
{
    public class ComposedSceneLoadResult
    {
        public ComposedSceneLoadResult(
            IReadOnlyList<SceneLoadResult> scenes,
            IReadOnlyList<ILocalSwitchCallbackListener> switchCallbacks,
            LifetimeScope scope,
            Action loadedCallback)
        {
            Scenes = scenes;
            _switchCallbacks = switchCallbacks;
            _scope = scope;
            _loadedCallback = loadedCallback;
        }

        private readonly Action _loadedCallback;
        private readonly LifetimeScope _scope;

        private readonly IReadOnlyList<ILocalSwitchCallbackListener> _switchCallbacks;

        public readonly IReadOnlyList<SceneLoadResult> Scenes;

        public void OnLoaded()
        {
            _loadedCallback?.Invoke();
        }

        public void OnUnload()
        {
            _scope.Dispose();

            foreach (var switchCallback in _switchCallbacks)
                switchCallback?.OnDisabled();
        }
    }
}