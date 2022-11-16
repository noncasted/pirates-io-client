﻿#region

using System;
using GamePlay.Items.Abstract;

#endregion

namespace GamePlay.Services.Common.InventoryGrids
{
    public interface IInventoryGrid
    {
        event Action<IItem> Selected;
        event Action Deselected;

        void Fill(IItem[] items);
    }
}