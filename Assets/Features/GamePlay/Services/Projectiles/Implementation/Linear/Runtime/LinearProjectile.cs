﻿using System;
using Common.ObjectsPools.Runtime.Abstract;
using Common.Structs;
using Cysharp.Threading.Tasks;
using GamePlay.Services.Projectiles.Entity;
using GamePlay.Services.Projectiles.Mover;
using GamePlay.Services.Projectiles.Mover.Abstract;
using GamePlay.Services.VFX.Pool.Implementation.Animated;
using NaughtyAttributes;
using UnityEngine;

namespace GamePlay.Services.Projectiles.Implementation.Linear.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider2D))]
    public class LinearProjectile :
        MonoBehaviour,
        IProjectileStarter,
        IPoolObject<LinearProjectile>,
        IMovableProjectile
    {
        public void Construct(IProjectilesMover mover, IObjectProvider<AnimatedVfx> vfx)
        {
            _vfx = vfx;
            _mover = mover;
            _transform = transform;
        }

        [SerializeField] [ReadOnly] private ShootParams _shootParams;
        [SerializeField] [ReadOnly] private bool _isLocal;

        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private TrailRenderer _trail;
        
        private Actions _actions;
        private Movement _movement;

        private IProjectilesMover _mover;

        private Action<LinearProjectile> _returnCallback;
        private Transform _transform;
        private IObjectProvider<AnimatedVfx> _vfx;
        public IProjectileMovement Movement => _movement;
        public IProjectileActions Actions => _actions;

        public GameObject GameObject => gameObject;

        public void SetPosition(Vector2 position)
        {
            _transform.position = position;
        }

        public void SetupPoolObject(Action<LinearProjectile> returnToPool)
        {
            _returnCallback = returnToPool;

            var raycastData = new ProjectileRaycastData(_collider.size.y);

            _actions = new Actions(
                _transform,
                this,
                returnToPool,
                OnTriggered,
                OnCollided,
                OnDropped,
                _mover);

            _movement = new Movement(
                raycastData,
                _transform,
                _actions.OnDropped);
        }
        
        public void Fire(
            Vector2 direction,
            ShootParams shootParams,
            bool isLocal,
            string creatorId)
        {
            _shootParams = shootParams;
            _isLocal = isLocal;

            var movementData = new MovementData(
                direction,
                shootParams.Speed,
                shootParams.Distance);

            var angle = direction.ToAngle();

            _movement.Setup(angle, movementData);
            _actions.Setup(shootParams, isLocal, creatorId);
            _collider.enabled = true;

            _mover.Add(this);

            _spriteRenderer.enabled = true;
        }

        public void OnTaken()
        {
            _trail.Clear();
        }

        public void OnReturned()
        {
            _trail.Clear();
        }

        private void OnCollided() => OnCollidedAsync().Forget();
        private void OnTriggered() => OnTriggeredAsync().Forget();
        private void OnDropped() => OnDroppedAsync().Forget();

        private async UniTaskVoid OnCollidedAsync()
        {
            _spriteRenderer.enabled = false;
            _particleSystem.Stop();
            _collider.enabled = false;
            
            var cancellation = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(3000, false, PlayerLoopTiming.Update, cancellation);
            
            _returnCallback?.Invoke(this);
        }

        private async UniTaskVoid OnTriggeredAsync()
        {
            _spriteRenderer.enabled = false;
            _particleSystem.Stop();
            _collider.enabled = false;

            var cancellation = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(3000, false, PlayerLoopTiming.Update, cancellation);
            
            _returnCallback?.Invoke(this);
        }

        private async UniTaskVoid OnDroppedAsync()
        {
            _spriteRenderer.enabled = false;
            _particleSystem.Stop();
            _vfx.Get(transform.position);
            _collider.enabled = false;

            var cancellation = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(3000, false, PlayerLoopTiming.Update, cancellation);
            
            _returnCallback?.Invoke(this);
        }
    }
}