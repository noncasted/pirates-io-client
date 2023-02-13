﻿using Features.GamePlay.Player.Entity.Network.Common.Events;
using GamePlay.Player.Entity.Network.Root.Runtime;
using GamePlay.Player.Entity.Setup.Flow.Callbacks;
using GamePlay.Services.Network.RemoteEntities.Storage;
using Ragon.Client;
using UnityEngine;

namespace Features.GamePlay.Player.Entity.Network.Local.View.Actions
{
    public class LocalPlayerActions : IPlayerSwitchListener
    {
        public LocalPlayerActions(
            IPlayerEventListener listener,
            IRemotePlayersRegistry remotePlayersRegistry)
        {
            _listener = listener;
            _remotePlayersRegistry = remotePlayersRegistry;
        }

        private readonly IPlayerEventListener _listener;
        private readonly IRemotePlayersRegistry _remotePlayersRegistry;

        public void OnEnabled()
        {
            _listener.AddListener<StartBoardingNetworkEvent>(OnBoardingStarted);
        }

        public void OnDisabled()
        {
        }

        private void OnBoardingStarted(RagonPlayer player, StartBoardingNetworkEvent data)
        {
            if (_remotePlayersRegistry.TryGet(player.Id, out var opponent) == false)
            {
                Debug.LogError($"No boarding opponent found with id: {player.Id}");
                return;
            }
            
            
        }
    }
}