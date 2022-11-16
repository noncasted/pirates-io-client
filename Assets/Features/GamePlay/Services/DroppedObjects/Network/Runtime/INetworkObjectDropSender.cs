﻿using GamePlay.Items.Abstract;
using UnityEngine;

namespace GamePlay.Services.DroppedObjects.Network.Runtime
{
    public interface INetworkObjectDropSender
    {
        void OnItemDropped(IItem item, Vector2 position);
        void OnItemCollected(int id);
    }
}