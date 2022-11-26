﻿using Common.EditableScriptableObjects.Attributes;
using GamePlay.Player.Entity.Components.Abstract;
using GamePlay.Player.Entity.Setup.Flow.Callbacks;
using GamePlay.Player.Entity.Setup.Path;
using GamePlay.Player.Entity.States.RangeAttacks.Logs;
using GamePlay.Player.Entity.States.RangeAttacks.Runtime.Attack;
using GamePlay.Player.Entity.States.RangeAttacks.Runtime.Config;
using UnityEngine;
using VContainer;

namespace GamePlay.Player.Entity.States.RangeAttacks.Runtime
{
    [CreateAssetMenu(fileName = PlayerAssetsPaths.StatePrefix + "RangeAttack",
        menuName = PlayerAssetsPaths.RangeAttack + "State")]
    public class RangeAttackAsset : PlayerComponentAsset
    {
        [SerializeField]  private RangeAttackConfigAsset _config;
        [SerializeField]  private RangeAttackDefinition _definition;
        [SerializeField]  private RangeAttackLogSettings _logSettings;

        public override void Register(IContainerBuilder builder)
        {
            builder.Register<RangeAttackLogger>(Lifetime.Scoped)
                .WithParameter(_logSettings);

            builder.Register<RangeAttack>(Lifetime.Scoped)
                .WithParameter(_definition)
                .As<IRangeAttack>()
                .AsSelf();

            builder.Register<RangeAttackInput>(Lifetime.Scoped).AsSelf();

            builder.Register<RangeAttackConfig>(Lifetime.Scoped)
                .WithParameter(_config)
                .As<IRangeAttackConfig>();
        }

        public override void Resolve(IObjectResolver resolver, ICallbackRegister callbackRegister)
        {
            callbackRegister.Add(resolver.Resolve<RangeAttackInput>());
            callbackRegister.Add(resolver.Resolve<RangeAttack>());
        }
    }
}