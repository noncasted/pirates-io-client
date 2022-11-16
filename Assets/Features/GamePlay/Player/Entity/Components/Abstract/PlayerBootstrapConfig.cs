﻿#region

using UnityEngine;

#endregion

namespace GamePlay.Player.Entity.Components.Abstract
{
    public abstract class PlayerBootstrapConfig : ScriptableObject
    {
        public abstract PlayerComponentAsset[] GetAssets();
    }
}