﻿using System;
using FMOD.Studio;
using FMODUnity;
using GamePlay.Services.Projectiles.Entity;
using UnityEngine;

namespace Global.Services.Sounds.Runtime

{
    [DisallowMultipleComponent]
    public class SoundsPlayer : MonoBehaviour
    {
        [Space(30)] [Header("Battle")] [SerializeField]
        public EventReference ShotEvent;
        public FMOD.Studio.EventInstance ShotInstance;

        public EventReference DamageEvent;
        public FMOD.Studio.EventInstance DamageInstance;

        public EventReference DamageReceivedEvent;
        public FMOD.Studio.EventInstance DamageReceivedInstance;

        [Space(30)] [Header("Ambience")] [SerializeField]
        public EventReference AmbEvent;
        public FMOD.Studio.EventInstance AmbInstance;

        [Space(30)] [Header("Music")] [SerializeField]
        public EventReference MusicEvent;
        public FMOD.Studio.EventInstance MusicInstance;

        [Space(30)] [Header("UI")] [SerializeField]
        public EventReference UiOpenedEvent;
        public EventReference ButtonClickedEvent;
        public EventReference OverButtonEvent;
        public EventReference MenuEnteredEvent;
        public EventReference MenuExitedEvent;

        [Space(30)]
        [Header("FX")]
        [SerializeField]
        public EventReference BurningEvent;
        public FMOD.Studio.EventInstance BurningInstance;

        private float _health;

        private Transform _fmodInstance;

        private void Start()
        {
            AmbInstance = RuntimeManager.CreateInstance(AmbEvent);
            AmbInstance.start();

            MusicInstance = RuntimeManager.CreateInstance(MusicEvent);
            MusicInstance.start();

            var fmodObject = new GameObject("FmodInstance");
            _fmodInstance = fmodObject.transform;
        }

        //Amb
        public void OnCityExited()
        {
            RuntimeManager.StudioSystem.setParameterByName("amb_condition", 0f);
            Debug.Log("open sea");
        }

        public void OnPortEntered()
        {
            RuntimeManager.StudioSystem.setParameterByName("music_condition", 1f);
            AmbInstance.setParameterByName("amb_condition", 1f);
            Debug.Log("port_enter");
        }

        public void OnCityEntered()
        {
            RuntimeManager.StudioSystem.setParameterByName("music_condition", 0f);
            AmbInstance.setParameterByName("amb_condition", 2f);
            Debug.Log("city entered");
        }

        public void OnPortExited()
        {
            //AmbInstance.setParameterByName("amb_condition", 2f);
            Debug.Log("port exit");
        }

        //Music
        public void OnBattleEntered()
        {
            RuntimeManager.StudioSystem.setParameterByName("music_condition", 2f);
            Debug.Log("BattleEntered");
        }

        public void OnBattleExited()
        {
            RuntimeManager.StudioSystem.setParameterByName("music_condition", 0f);
            Debug.Log("BattleExited");
        }

        //Battle
        public void OnCannonBallShot(Vector2 position)
        {
            PlayShot(0f, position);
            Debug.Log("boom CannonBall");
        }

        public void OnShrapnelShot(Vector2 position)
        {
            PlayShot(1f, position);
            Debug.Log("boom Shrapnel");
        }

        public void OnKnuppelShot(Vector2 position)
        {
            PlayShot(2f, position);
            Debug.Log("boom Knuppel");
        }

        private void PlayShot(float parameter, Vector2 position)
        {
            ShotInstance = RuntimeManager.CreateInstance(ShotEvent);
            AttachInstance(ShotInstance, position);
            ShotInstance.setParameterByName("shot_type", parameter);
            ShotInstance.start();
            ShotInstance.release();
        }

        //Damage
        public void OnProjectileDropped(Vector2 position)
        {
            ////PlayDamage(0f, position);
            //DamageInstance = RuntimeManager.CreateInstance(DamageEvent);
            //DamageInstance.setParameterByName("damage_type", 0f);
            //RuntimeManager.PlayOneShot(DamageEvent);
        }

        public void OnDeath(Vector2 position)
        {
        }

        public void OnEnemyDamaged(GameObject target, ProjectileType type) //реализовать переключение типа урона
        {
            switch (type)
            {
                case ProjectileType.Ball:
                {                       
                        
                    break;
                }
                case ProjectileType.Knuppel:
                {
                        
                        break;
                }
                case ProjectileType.Shrapnel:
                {
                       
                        break;
                }
                case ProjectileType.Fishnet:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }                            
           RuntimeManager.PlayOneShotAttached(DamageEvent, target);          
        }

        public void OnDamageReceived()
        {
            RuntimeManager.PlayOneShot(DamageReceivedEvent);
            Debug.Log("We are damaged!");
        }

        

        //UI
        public void OnUiOpened()
        {
            RuntimeManager.PlayOneShot(UiOpenedEvent);
        }

        public void OnOverButton()
        {
            RuntimeManager.PlayOneShot(OverButtonEvent);
        }

        public void OnButtonClicked()
        {
            RuntimeManager.PlayOneShot(ButtonClickedEvent);
        }

        public void OnMenuEntered()
        {
            RuntimeManager.PlayOneShot(MenuEnteredEvent);
        }

        public void OnMenuExited()
        {
            RuntimeManager.PlayOneShot(MenuExitedEvent);
        }

        //HP
        public void OnHealthChanged(float health)
        {
            _health = health;
            
            if (health < 0.5)
            {
                MusicInstance.setParameterByName("music_intencity", 2f);
                //Burning();
            }
            else
            {
                BurningInstance.release();

            }

        //void Burning(GameObject target)
        //    {
        //        BurningInstance = RuntimeManager.CreateInstance(BurningEvent);
        //        AttachInstance(BurningInstance, target);
        //        BurningInstance.setParameterByName("health", 0.5f);
        //        BurningInstance.start();
        //    }


        }

        private void AttachInstance(EventInstance instance, Vector2 position)
        {
            _fmodInstance.position = position;
            RuntimeManager.AttachInstanceToGameObject(instance, _fmodInstance);
        }
    }
}