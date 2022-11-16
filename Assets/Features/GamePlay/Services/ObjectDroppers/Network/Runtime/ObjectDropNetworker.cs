﻿#region

using System;
using GamePlay.Items.Abstract;
using GamePlay.Services.Network.Common.EntityProvider.Runtime;
using GamePlay.Services.Network.PlayerDataProvider.Runtime;
using Ragon.Client;
using Ragon.Common;
using UnityEngine;
using VContainer;

#endregion

namespace GamePlay.Services.ObjectDroppers.Network.Runtime
{
    public class ObjectDropNetworker :
        MonoBehaviour,
        INetworkObjectDropSender,
        INetworkObjectDropReceiver
    {
        [Inject]
        private void Construct(
            INetworkSessionEventSender sender,
            INetworkSessionEventListener listener,
            INetworkPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
            _sender = sender;
            _listener = listener;

            _listener.AddListener<ItemDropEvent>(OnItemDropReceived);
            _listener.AddListener<ItemCollectedEvent>(OnItemCollectReceived);
        }

        private INetworkSessionEventSender _sender;
        private INetworkSessionEventListener _listener;
        private INetworkPlayerDataProvider _playerDataProvider;

        public event Action<ItemDropEvent> ItemDropped;
        public event Action<int> ItemCollected;

        public void OnItemDropped(IItem item, Vector2 position)
        {
            var data = new ItemDropEvent(
                item.BaseData.Type,
                position,
                item.Count,
                _playerDataProvider.GenerateUniqueId());

            _sender.ReplicateEvent(data, RagonTarget.All, RagonReplicationMode.LocalAndServer);
        }

        public void OnItemCollected(int id)
        {
            var data = new ItemCollectedEvent(id);

            _sender.ReplicateEvent(data, RagonTarget.All, RagonReplicationMode.Server);
        }

        private void OnItemDropReceived(RagonPlayer player, ItemDropEvent data)
        {
            ItemDropped?.Invoke(data);
        }

        private void OnItemCollectReceived(RagonPlayer player, ItemCollectedEvent data)
        {
            ItemCollected?.Invoke(data.Id);
        }
    }
}