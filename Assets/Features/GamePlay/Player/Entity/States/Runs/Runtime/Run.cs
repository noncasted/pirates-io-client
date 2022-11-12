﻿using GamePlay.Player.Entity.Components.InertialMovements.Runtime;
using GamePlay.Player.Entity.Components.Rotations.Runtime.Abstract;
using GamePlay.Player.Entity.Components.StateMachines.Runtime;
using GamePlay.Player.Entity.States.Abstract;
using GamePlay.Player.Entity.States.Common;
using GamePlay.Player.Entity.States.Runs.Logs;
using UnityEngine;

namespace GamePlay.Player.Entity.States.Runs.Runtime
{
    public class Run : IRun, IState
    {
        public Run(
            IStateMachine stateMachine,
            IInertialMovement inertialMovement,
            IRunConfig config,
            ISpriteRotation spriteRotation,
            StateDefinition definition,
            RunLogger logger)
        {
            _stateMachine = stateMachine;
            _inertialMovement = inertialMovement;
            _config = config;

            _spriteRotation = spriteRotation;

            Definition = definition;
            _logger = logger;
        }

        private readonly RunLogger _logger;
        private readonly IRunConfig _config;

        private readonly ISpriteRotation _spriteRotation;

        private readonly IStateMachine _stateMachine;
        private readonly IInertialMovement _inertialMovement;

        private Vector2 _input;
        private bool _isStarted;

        public bool HasInput => _input != Vector2.zero;
        public StateDefinition Definition { get; }

        public void OnInput(Vector2 input)
        {
            _input = input;

            if (_isStarted == true)
                return;

            if (_stateMachine.IsAvailable(Definition) == false)
                return;

            _logger.OnEnteredOnTrigger(input);

            Begin();
        }

        public void OnCancel()
        {
            _input = Vector2.zero;

            if (_isStarted == false)
                return;

            _inertialMovement.Disable();
            _stateMachine.Exit();
        }

        public void Enter()
        {
            if (_input == Vector2.zero || _isStarted == true)
            {
                _logger.OnEnterFromFloatingError();
                return;
            }

            _logger.OnEnteredFromFloating(_input);

            Begin();
        }

        public void Break()
        {
            if (_isStarted == false)
                return;

            _isStarted = false;
            _spriteRotation.Stop();

            _logger.OnBroke();
        }

        private void Begin()
        {
            _stateMachine.Enter(this);
            
            _inertialMovement.Enable();
            _inertialMovement.SetSpeed(_config.Speed);
            _spriteRotation.Start();
            _isStarted = true;
        }
    }
}