﻿using System;
using Common.ObjectsPools.Runtime.Abstract;
using Common.Structs;
using GamePlay.Player.Entity.Components.Healths.Runtime;
using GamePlay.Player.Entity.Network.Remote.Receivers.Damages.Runtime;
using GamePlay.Player.Entity.Network.Root.Runtime;
using GamePlay.Player.Entity.States.Deaths.Runtime;
using GamePlay.Player.Entity.Views.Sprites.Runtime;
using GamePlay.Player.Entity.Views.Transforms.Runtime;
using GamePlay.Services.Projectiles.Entity;
using GamePlay.Services.VFX.Pool.Implementation.Animated;
using GamePlay.Services.VFX.Pool.Provider;
using Ragon.Client;

namespace GamePlay.Player.Entity.Components.DamageProcessors.Runtime
{
    public class DamageProcessor
    {
        public DamageProcessor(
            IHealth health,
            ISail sail,
            IDeath death,
            IPlayerEventListener listener,
            ISpriteFlash flash,
            IVfxPoolProvider vfxPool,
            IBodyTransform transform,
            DamageConfigAsset config)
        {
            _health = health;
            _sail = sail;
            _death = death;
            _flash = flash;
            _transform = transform;
            _config = config;
            _explosion = vfxPool.GetPool<AnimatedVfx>(_config.Explosion);

            listener.AddListener<DamageEvent>(OnDamageReceived);
        }

        private readonly DamageConfigAsset _config;
        private readonly IDeath _death;
        private readonly IObjectProvider<AnimatedVfx> _explosion;
        private readonly ISpriteFlash _flash;

        private readonly IHealth _health;
        private readonly ISail _sail;
        private readonly IBodyTransform _transform;

        private void OnDamageReceived(RagonPlayer player, DamageEvent damage)
        {
            _health.ApplyDamage(damage.Amount);

            var explosion = _explosion.Get(damage.Origin);
            var direction = damage.Origin - _transform.Position;
            direction.Normalize();
            explosion.transform.RotateAlong(direction);

            switch (damage.Type)
            {
                case ProjectileType.Ball:
                    _sail.Damage(0.2f);
                    break;
                case ProjectileType.Knuppel:
                    _sail.Damage(1f);
                    break;
                case ProjectileType.Shrapnel:
                    _sail.Damage(0.1f);
                    break;
                case ProjectileType.Fishnet:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_health.IsAlive == false)
                _death.Enter();
            else
                _flash.Flash(_config.FlashTime);
        }
    }
}