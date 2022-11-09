﻿using Common.EditableScriptableObjects.Attributes;
using GamePlay.Player.Entity.Components.Rotations.Runtime;
using GamePlay.Player.Entity.Setup.Path;
using GamePlay.Player.Entity.States.Idles.Runtime;
using GamePlay.Player.Entity.States.RangeAttacks.Runtime;
using GamePlay.Player.Entity.States.Respawns.Runtime;
using GamePlay.Player.Entity.States.Runs.Runtime;
using UnityEngine;

namespace GamePlay.Player.Entity.Setup.Editor
{
    [CreateAssetMenu(fileName = PlayerAssetsPaths.ConfigPrefix + "Animations",
        menuName = PlayerAssetsPaths.Config + "Animations")]
    public class PlayerAnimationsConfig : ScriptableObject
    {
        [SerializeField] [EditableObject] private IdleAnimationTriggerAsset _idle;
        [SerializeField] [EditableObject] private RespawnAnimationTriggerAsset _respawn;
        [SerializeField] [EditableObject] private RunAnimationTriggerAsset _run;
        [SerializeField] [EditableObject] private RangeAttackAnimationTriggerAsset _rangeAttack;
        [SerializeField] [EditableObject] private RotationAnimatorFloatAsset _rotation;
    }
}