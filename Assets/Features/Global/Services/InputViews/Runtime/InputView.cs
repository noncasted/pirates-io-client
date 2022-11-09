﻿using System;
using Global.Services.CameraUtilities.Runtime;
using Global.Services.Common.Abstract;
using Global.Services.InputViews.Logs;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Global.Services.InputViews.Runtime
{
    public class InputView : MonoBehaviour, IInputView, IInputViewRebindCallbacks, IGlobalServiceAwakeListener
    {
        [Inject]
        private void Construct(
            InputViewLogger logger,
            ICameraUtils cameraUtils)
        {
            _logger = logger;
            _cameraUtils = cameraUtils;
        }

        private ICameraUtils _cameraUtils;

        private Controls _controls;
        private Controls.GamePlayActions _gamePlay;
        private Controls.DebugActions _debug;

        private InputViewLogger _logger;

        private void OnDestroy()
        {
            UnListen();

            _controls.Disable();
        }

        public void OnAwake()
        {
            _controls = new Controls();
            _controls.Enable();
            
            _gamePlay = _controls.GamePlay;
            _debug = _controls.Debug;

            Listen();
        }

        public event Action<Vector2> MovementPerformed;
        public event Action MovementCanceled;
        public event Action RangeAttackPerformed;
        public event Action RangeAttackCanceled;
        public event Action DebugConsolePreformed;
        

        public float GetAngleFrom(Vector2 from)
        {
            var direction = GetDirectionFrom(from);

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0f)
                angle += 360f;

            return angle;
        }

        public Vector2 GetDirectionFrom(Vector2 from)
        {
            var screenPosition = Mouse.current.position.ReadValue();
            var worldPosition = _cameraUtils.ScreenToWorld(screenPosition);

            var direction = worldPosition - from;
            direction.Normalize();

            return direction;
        }

        public LineResult GetLineFrom(Vector2 from)
        {
            var screenPosition = Mouse.current.position.ReadValue();
            var worldPosition = _cameraUtils.ScreenToWorld(screenPosition);

            var direction = worldPosition - from;
            direction.Normalize();

            var length = Vector2.Distance(from, worldPosition);

            return new LineResult(direction, length);
        }

        public void OnBeforeRebind()
        {
            _logger.OnBeforeRebind();
        }

        public void OnAfterRebind()
        {
            _logger.OnAfterRebind();
        }

        private void Listen()
        {
            _gamePlay.Movement.performed += OnMovementPerformed;
            _gamePlay.Movement.canceled += OnMovementCanceled;

            _gamePlay.RangeAttack.performed += OnRangeAttackPerformed;
            _gamePlay.RangeAttack.canceled += OnRangeAttackCanceled;

            _debug.Console.performed += OnDebugConsolePreformed;
        }

        private void UnListen()
        {
            _gamePlay.Movement.performed -= OnMovementPerformed;
            _gamePlay.Movement.canceled -= OnMovementCanceled;

            _gamePlay.RangeAttack.performed -= OnRangeAttackPerformed;
            _gamePlay.RangeAttack.canceled -= OnRangeAttackCanceled;
            
            _debug.Console.performed -= OnDebugConsolePreformed;
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();

            _logger.OnMovementPressed(value);

            MovementPerformed?.Invoke(value);
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _logger.OnMovementCanceled();

            MovementCanceled?.Invoke();
        }

        private void OnRangeAttackPerformed(InputAction.CallbackContext context)
        {
            _logger.OnRangeAttackPerformed();

            RangeAttackPerformed?.Invoke();
        }

        private void OnRangeAttackCanceled(InputAction.CallbackContext context)
        {
            _logger.OnRangeAttackCanceled();

            RangeAttackCanceled?.Invoke();
        }

        private void OnDebugConsolePreformed(InputAction.CallbackContext context)
        {
            DebugConsolePreformed?.Invoke();
        }
    }
}