﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Local.Services.Abstract;
using Local.Services.Abstract.Callbacks;
using Local.Services.DependenciesResolve;
using VContainer;

namespace Local.ComposedSceneConfig
{
    public class CallbacksRegister : ICallbacksRegister
    {
        private readonly List<ILocalAsyncAwakeListener> _asyncAwakes = new();
        private readonly List<ILocalAwakeCallbackListener> _awakes = new();
        private readonly List<ILocalLoadCallbackListener> _loads = new();
        private readonly List<IDependencyRegister> _registers = new();
        private readonly List<IDependencyResolver> _resolvers = new();
        private readonly List<ILocalSwitchCallbackListener> _switches = new();

        public IReadOnlyList<ILocalSwitchCallbackListener> SwitchCallbacks => _switches;

        public void ListenFlowCallbacks(object service)
        {
            if (service is ILocalAwakeCallbackListener awake)
                _awakes.Add(awake);

            if (service is ILocalAsyncAwakeListener asyncAwake)
                _asyncAwakes.Add(asyncAwake);

            if (service is ILocalSwitchCallbackListener switchCallback)
                _switches.Add(switchCallback);

            if (service is ILocalLoadCallbackListener bootstrap)
                _loads.Add(bootstrap);
        }

        public void ListenContainerCallbacks(object service)
        {
            if (service is IDependencyRegister register)
                _registers.Add(register);

            if (service is IDependencyResolver resolver)
                _resolvers.Add(resolver);
        }

        public void InvokeAwakeCallbacks()
        {
            foreach (var awake in _awakes)
                awake.OnAwake();
        }

        public async UniTask InvokeAsyncAwakeCallbacks()
        {
            var tasks = new UniTask[_asyncAwakes.Count];

            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = _asyncAwakes[i].OnAwakeAsync();

            await UniTask.WhenAll(tasks);
        }

        public void InvokeEnableCallback()
        {
            foreach (var switchListener in _switches)
                switchListener.OnEnabled();
        }

        public void InvokeLoadedCallbacks()
        {
            foreach (var load in _loads)
                load.OnLoaded();
        }

        public void Resolve(IObjectResolver objectResolver)
        {
            foreach (var resolver in _resolvers)
                resolver.Resolve(objectResolver);
        }

        public void InvokeRegisterCallbacks(IContainerBuilder builder)
        {
            foreach (var register in _registers)
                register.Register(builder);
        }
    }
}