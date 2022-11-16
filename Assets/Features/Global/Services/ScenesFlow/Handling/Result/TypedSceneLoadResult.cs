﻿#region

using UnityEngine.ResourceManagement.ResourceProviders;

#endregion

namespace Global.Services.ScenesFlow.Handling.Result
{
    public class TypedSceneLoadResult<T> : SceneLoadResult
    {
        public TypedSceneLoadResult(SceneInstance instance, T searched) : base(instance)
        {
            Searched = searched;
        }

        public readonly T Searched;
    }
}