﻿#region

using GamePlay.Player.Entity.Setup.Flow.Callbacks;
using UnityEngine;

#endregion

namespace GamePlay.Player.Entity.Views.RotationPoint
{
    public class PlayerRotationPoint : MonoBehaviour, IAwakeCallback, IRotationPoint
    {
        private Transform _transform;

        public void OnAwake()
        {
            _transform = transform;
        }

        public Vector2 Position => _transform.position;
        public Transform Transform => _transform;
    }
}