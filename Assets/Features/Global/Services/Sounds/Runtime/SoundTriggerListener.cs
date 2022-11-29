﻿using System;
using GamePlay.Player.Entity.Components.Healths.Runtime;
using UniRx;
using UnityEngine;

namespace Global.Services.Sounds.Runtime
{
    public class SoundTriggerListener : MonoBehaviour
    {
        [SerializeField] private SoundsPlayer _player;

        private IDisposable _triggerListener;
        private IDisposable _positionalTriggerListener;
        private IDisposable _healthListener;
        private IDisposable _damageListener;

        private void OnEnable()
        {
            _triggerListener = MessageBroker.Default.Receive<SoundEvent>().Subscribe(OnSoundTriggered);
            _positionalTriggerListener = MessageBroker.Default.Receive<PositionalSoundEvent>().Subscribe(OnPositionalSoundTriggered);
            _healthListener = MessageBroker.Default.Receive<HealthChangeEvent>().Subscribe(OnHealthChanged);
            _damageListener = MessageBroker.Default.Receive<EnemyDamagedSoundEvent>().Subscribe(OnEnemyDamaged);
        }

        private void OnDisable()
        {
            _triggerListener?.Dispose();
            _positionalTriggerListener?.Dispose();
            _healthListener?.Dispose();
            _damageListener?.Dispose();
        }

        private void OnSoundTriggered(SoundEvent data)
        {
            switch (data.Type)
            {
                case SoundType.CityEnter:
                    _player.OnCityEntered();
                    break;
                case SoundType.CityExit:
                    _player.OnCityExited();
                    break;
                case SoundType.PortEnter:
                    _player.OnPortEntered();
                    break;
                case SoundType.PortExit:
                    _player.OnPortExited();
                    break;
                case SoundType.BattleEnter:
                    _player.OnBattleEntered();
                    break;
                case SoundType.BattleExit:
                    _player.OnBattleExited();
                    break;
                case SoundType.UiOpen:
                    _player.OnUiOpened();
                    break;
                case SoundType.OverButton:
                    _player.OnOverButton();
                    break;
                case SoundType.ButtonClick:
                    _player.OnButtonClicked();
                    break;
                case SoundType.MenuEntered:
                    _player.OnMenuEntered();
                    break;
                case SoundType.MenuExited:
                    _player.OnMenuExited();
                    break;
                default:
                    Debug.LogError($"Sound: {data.Type} is not implemented");
                    break;
            }
        }

        private void OnPositionalSoundTriggered(PositionalSoundEvent data)
        {
            switch (data.Type)
            {
                case PositionalSoundType.CannonBallShot:
                    _player.OnCannonBallShot(data.Position);
                    break;
                case PositionalSoundType.ShrapnelShot:
                    _player.OnShrapnelShot(data.Position);
                    break;
                case PositionalSoundType.KnuppelShot:
                    _player.OnKnuppelShot(data.Position);
                    break;
                case PositionalSoundType.ProjectileDropped:
                    _player.OnProjectileDropped(data.Position);
                    break;
                case PositionalSoundType.EnemyDamaged:
                    break;
                case PositionalSoundType.DamageReceived:
                    _player.OnDamageReceived();
                    break;
                case PositionalSoundType.Death:
                    _player.OnDeath(data.Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnHealthChanged(HealthChangeEvent data)
        {
            var delta = data.Current / data.Max;
            
            _player.OnHealthChanged(delta);
        }

        private void OnEnemyDamaged(EnemyDamagedSoundEvent data)
        {
            _player.OnEnemyDamaged(data.Target, data.Type);
        }
    }
}