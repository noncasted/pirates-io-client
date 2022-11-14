﻿using Features.GamePlay.Common.Areas.Common;
using GamePlay.Player.Entity.Components.ActionsStates.Runtime;
using UnityEngine;
using VContainer;

namespace GamePlay.Player.Entity.Network.Local.AreaInteractors.Runtime
{
    [DisallowMultipleComponent]
    public class LocalAreaInteractor : MonoBehaviour, IAreaInteractor
    {
        [Inject]
        private void Construct(IActionsStatePresenter actionsStatePresenter)
        {
            _actionsStatePresenter = actionsStatePresenter;
        }

        private IActionsStatePresenter _actionsStatePresenter;
        
        public void OnCityEntered()
        {
            _actionsStatePresenter.DisableShooting();            
        }

        public void OnAreaExited()
        {
            _actionsStatePresenter.EnableShooting();            
        }
    }
}