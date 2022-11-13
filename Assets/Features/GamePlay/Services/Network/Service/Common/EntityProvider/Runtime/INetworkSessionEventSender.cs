﻿using Ragon.Client;
using Ragon.Common;

namespace GamePlay.Services.Network.Service.Common.EntityProvider.Runtime
{
    public interface INetworkSessionEventSender
    {
        void ReplicateEvent<TEvent>(
            TEvent data,
            RagonTarget target,
            RagonReplicationMode replicationMode)
            where TEvent : IRagonEvent, new();
    }
}