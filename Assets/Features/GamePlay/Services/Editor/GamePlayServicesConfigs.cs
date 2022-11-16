﻿using Common.EditableScriptableObjects.Attributes;
using GamePlay.Common.Paths;
using GamePlay.Services.LevelCameras.Runtime;
using GamePlay.Services.Projectiles.Mover;
using GamePlay.Services.TransitionScreens.Runtime;
using UnityEngine;

namespace GamePlay.Services.Editor
{
    [CreateAssetMenu(fileName = GamePlayAssetsPaths.ConfigPrefix + "ServicesConfigs",
        menuName = GamePlayAssetsPaths.Config + "ServicesConfigs")]
    public class GamePlayServicesConfigs : ScriptableObject
    {
        [SerializeField] [EditableObject] private LevelCameraConfigAsset _levelCamera;
        [SerializeField] [EditableObject] private ProjectilesMoverConfigAsset _projectiles;
        [SerializeField] [EditableObject] private TransitionScreenConfigAsset _transitionScreen;
    }
}