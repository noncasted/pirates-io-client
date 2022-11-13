﻿using System.Collections.Generic;
using GamePlay.Common.Paths;
using GamePlay.Services.Projectiles.Entity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GamePlay.Services.Projectiles.Replicator.Runtime
{
    [CreateAssetMenu(fileName = GamePlayAssetsPaths.ConfigPrefix + "ProjectilesReplicator",
        menuName = GamePlayAssetsPaths.ProjectilesReplicator + "Config")]
    public class ProjectileReplicatorConfigAsset : ScriptableObject
    {
        [SerializeField] [Min(0f)] private float _replicateDistance = 40f;
        [SerializeField] private ProjectileTypeDictionary _projectiles;
        [SerializeField] private AssetReference _vfx;

        public float ReplicateDistance => _replicateDistance;
        public AssetReference Vfx => _vfx;
        public IReadOnlyDictionary<ProjectileType, AssetReference> Projectiles => _projectiles;
    }
}