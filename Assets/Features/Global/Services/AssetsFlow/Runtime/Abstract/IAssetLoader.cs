﻿#region

using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

#endregion

namespace Global.Services.AssetsFlow.Runtime.Abstract
{
    public interface IAssetLoader
    {
        UniTask<AssetLoadResult<T>> Load<T>(AssetReference reference);
    }
}