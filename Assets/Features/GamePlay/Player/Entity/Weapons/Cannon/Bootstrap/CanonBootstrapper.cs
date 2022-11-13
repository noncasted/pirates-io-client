﻿using Cysharp.Threading.Tasks;
using GamePlay.Player.Entity.Setup.Bootstrap;
using GamePlay.Player.Entity.Setup.Flow;
using GamePlay.Player.Entity.Weapons.Common.Bootstrap.Runtime;
using GamePlay.Player.Entity.Weapons.Common.Root;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Player.Entity.Weapons.Cannon.Bootstrap
{
    [DisallowMultipleComponent]
    public class CanonBootstrapper : MonoBehaviour, IWeaponBootstrapper
    {
        private IPlayerContainerBuilder[] _builders;

        public void ToChild(Transform parent)
        {
            transform.parent = parent;
        }

        public async UniTask<IWeapon> Build(LifetimeScope parent)
        {
            _builders = GetComponents<IPlayerContainerBuilder>();
            var scope = GetComponent<WeaponScope>();

            using (LifetimeScope.EnqueueParent(parent))
            {
                using (LifetimeScope.Enqueue(OnConfiguration))
                {
                    await UniTask.Create(async () => scope.Build());
                }
            }

            var flowHandler = new FlowHandler();

            foreach (var containerBuilder in _builders)
                containerBuilder.Resolve(scope.Container, flowHandler);

            var root = GetComponent<IWeapon>();

            await root.OnBootstrapped(flowHandler, scope);

            return root;
        }

        private void OnConfiguration(IContainerBuilder builder)
        {
            var root = GetComponent<IWeapon>();

            builder.RegisterComponent(root);

            foreach (var containerBuilder in _builders)
                containerBuilder.OnBuild(builder);
        }
    }
}