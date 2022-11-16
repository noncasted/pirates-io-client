﻿#region

using GamePlay.Items.Abstract;
using UnityEngine;

#endregion

namespace GamePlay.Services.ObjectDroppers.Network.Runtime
{
    public interface INetworkObjectDropSender
    {
        void OnItemDropped(IItem item, Vector2 position);
        void OnItemCollected(int id);
    }
}