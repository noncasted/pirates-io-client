﻿using System;
using UnityEngine;

namespace GamePlay.Services.Projectiles.Entity
{
    [Serializable]
    public class ShootParams
    {
        public ShootParams(
            int damage,
            float speed,
            float distance)
        {
            Damage = damage;
            Speed = speed;
            Distance = distance;

            _damage = damage;
            _speed = speed;
            _distance = distance;
        }
        
        [SerializeField] private int _shotsAmount;
        [SerializeField] private int _damage;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _speed;
        [SerializeField] private float _distance;
        [SerializeField] private LayerMask _layerMask;

        public readonly int Damage;
        public readonly float Speed;
        public readonly float Distance;
    }
}