﻿using Common.EditableScriptableObjects.Attributes;
using GamePlay.Player.Entity.Components.Abstract;
using GamePlay.Player.Entity.Setup.Flow.Callbacks;
using GamePlay.Player.Entity.Setup.Path;
using GamePlay.Player.Entity.States.Runs.Logs;
using GamePlay.Player.Entity.Views.Animators.Runtime;
using UnityEngine;
using VContainer;

namespace GamePlay.Player.Entity.States.Runs.Runtime
{
    [CreateAssetMenu(fileName = PlayerAssetsPaths.StatePrefix + "Run",
        menuName = PlayerAssetsPaths.Run + "State")]
    public class RunAsset : PlayerComponentAsset
    {
        [SerializeField] [EditableObject] private RunAnimationTriggerAsset _animation;
        [SerializeField] [EditableObject] private AnimatorFloatAsset _rotationFloat;
        [SerializeField] [EditableObject] private RunLogSettings _logSettings;
        [SerializeField] [EditableObject] private RunConfigAsset _config;
        [SerializeField] [EditableObject] private RunDefinition _definition;

        public override void Register(IContainerBuilder builder)
        {
            builder.Register<RunLogger>(Lifetime.Scoped)
                .WithParameter("settings", _logSettings);

            builder.Register<RunConfig>(Lifetime.Scoped)
                .WithParameter("asset", _config)
                .As<IRunConfig>();

            var animation = _animation.CreateTrigger();
            var rotation = _rotationFloat.CreateFloat();

            builder.Register<RunInput>(Lifetime.Scoped)
                .AsSelf();

            builder.Register<Run>(Lifetime.Scoped)
                .WithParameter("definition", _definition)
                .WithParameter("animation", animation)
                .WithParameter("rotationFloat", rotation)
                .As<IRun>();
        }

        public override void Resolve(IObjectResolver resolver, ICallbackRegister callbackRegister)
        {
            callbackRegister.Add(resolver.Resolve<RunInput>());
        }
    }
}