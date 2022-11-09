﻿using System;
using System.Collections.Generic;
using Global.Services.Common.Abstract;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Global.Bootstrappers
{
    public class ServiceBinder : IServiceBinder
    {
        public ServiceBinder(Scene scene)
        {
            _modulesTransformer = new ModulesTransformer(scene);
        }

        private readonly List<IGlobalServiceAwakeListener> _awakes = new();
        private readonly List<IGlobalServiceBootstrapListener> _bootstraps = new();
        private readonly List<Action<IContainerBuilder>> _builders = new();
        private readonly ModulesTransformer _modulesTransformer;

        public void AddToModules(MonoBehaviour service)
        {
            _modulesTransformer.AddModule(service);
        }

        public void ListenCallbacks(object service)
        {
            if (service is IGlobalServiceAwakeListener awake)
                _awakes.Add(awake);

            if (service is IGlobalServiceBootstrapListener bootstrap)
                _bootstraps.Add(bootstrap);
        }

        public void ListenBuildCallback(Action<IContainerBuilder> builder)
        {
            _builders.Add(builder);
        }

        public void InvokeFlowCallbacks()
        {
            foreach (var awake in _awakes)
                awake.OnAwake();

            foreach (var bootstrap in _bootstraps)
                bootstrap.OnBootstrapped();
        }

        public void InvokeBuilderCallbacks(IContainerBuilder builder)
        {
            foreach (var builderCallback in _builders)
                builderCallback?.Invoke(builder);
        }
    }
}