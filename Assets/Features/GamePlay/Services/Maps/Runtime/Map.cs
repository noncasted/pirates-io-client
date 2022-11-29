﻿using Global.Services.InputViews.Runtime;
using Global.Services.UiStateMachines.Runtime;
using Local.Services.Abstract.Callbacks;
using UnityEngine;
using VContainer;

namespace Features.GamePlay.Services.Maps.Runtime
{
    public class Map : MonoBehaviour, IMap, IUiState, ILocalSwitchListener
    {
        [Inject]
        private void Construct(
            IUiStateMachine uiStateMachine,
            IInputView inputView,
            UiConstraints constraints)
        {
            _inputView = inputView;
            _constraints = constraints;
            _uiStateMachine = uiStateMachine;
        }

        [SerializeField] private GameObject _body;
        [SerializeField] private MapPlayerMover _mover;
        
        private IUiStateMachine _uiStateMachine;
        private UiConstraints _constraints;
        private IInputView _inputView;

        public UiConstraints Constraints => _constraints;
        public string Name => "Map";
        public MapPlayerMover Mover => _mover;

        private void Awake()
        {
            _body.SetActive(false);
        }

        public void OnEnabled()
        {
            _inputView.MapPerformed += Switch;
        }

        public void OnDisabled()
        {
            _inputView.MapPerformed -= Switch;
        }

        public void Switch()
        {
            if (_body.activeSelf == true)
            {
                _uiStateMachine.Exit(this);
            }
            else
            {
                _uiStateMachine.EnterAsStack(this);

                _mover.OnOpened();
                _body.SetActive(true);
            }
        }

        public void Recover()
        {
            _mover.OnOpened();
            _body.SetActive(true);
        }

        public void Exit()
        {
            _mover.OnClosed();
            _body.SetActive(false);
        }
    }
}