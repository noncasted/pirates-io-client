﻿using GamePlay.Player.Entity.Setup.Flow.Callbacks;
using VContainer;

namespace GamePlay.Player.Entity.Setup.Bootstrap
{
    public interface IPlayerContainerBuilder
    {
        void OnBuild(IContainerBuilder builder);
        void Resolve(IObjectResolver resolver, ICallbackRegister callbackRegister);
    }
}