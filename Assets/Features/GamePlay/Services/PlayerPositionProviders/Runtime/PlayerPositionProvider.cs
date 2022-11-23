﻿using Ragon.Client;
using UnityEngine;

namespace GamePlay.Services.PlayerPositionProviders.Runtime
{
    public class PlayerPositionProvider : MonoBehaviour, IPlayerEntityPresenter, IPlayerPositionProvider
    {
        private RagonEntity _entity;
        private Transform _transform;

        public Vector2 Position => GetPosition();

        
        public void AssignPlayer(RagonEntity entity, Transform playerTransform)
        {
            _entity = entity;
            _transform = playerTransform;
        }

        public void DestroyPlayer()
        {
            RagonNetwork.Room.DestroyEntity(_entity.gameObject);
        }

        private Vector2 GetPosition()
        {
            if (_transform == null)
            {
                Debug.Log("No player assigned");
                return Vector2.zero;
            }

            return _transform.position;
        }
    }
}