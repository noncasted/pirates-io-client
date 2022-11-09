﻿using Common.EditableScriptableObjects.Attributes;
using Global.Common;
using Global.Services.Common.Abstract;
using Global.Services.Network.Connection.Logs;
using Global.Services.Network.Connection.Runtime;
using Global.Services.Network.Session.Join.Logs;
using Global.Services.Network.Session.Join.Runtime;
using Global.Services.Network.Session.Leave.Logs;
using Global.Services.Network.Session.Leave.Runtime;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Global.Services.Network.Bootstrap
{
    [CreateAssetMenu(fileName = GlobalAssetsPaths.ServicePrefix + "NetworkBootstrap",
        menuName = GlobalAssetsPaths.NetworkBootstrap)]
    public class NetworkAsset : GlobalServiceAsset
    {
        [SerializeField] [EditableObject] private NetworkConnectionConfigAsset _connectionConfig;
        [SerializeField] [EditableObject] private NetworkConnectorLogSettings _connectorLogSettings;
        [SerializeField] [EditableObject] private NetworkSessionJoinLogSettings _sessionJoinLogSettings;
        [SerializeField] [EditableObject] private NetworkSessionLeaveLogSettings _sessionLeaveLogSettings;

        [SerializeField] private NetworkConnector _prefab;

        public override void Create(IContainerBuilder builder, IServiceBinder serviceBinder)
        {
            var connector = Instantiate(_prefab);
            connector.name = "Network";

            var joiner = connector.GetComponent<NetworkSessionJoiner>();
            var leaver = connector.GetComponent<NetworkSessionLeaver>();

            builder.Register<NetworkConnectorLogger>(Lifetime.Scoped)
                .WithParameter("settings", _connectorLogSettings);

            builder.Register<NetworkSessionJoinLogger>(Lifetime.Scoped)
                .WithParameter("settings", _sessionJoinLogSettings);

            builder.Register<NetworkSessionLeaveLogger>(Lifetime.Scoped)
                .WithParameter("settings", _sessionLeaveLogSettings);

            builder.RegisterComponent(connector)
                .WithParameter("config", _connectionConfig)
                .AsImplementedInterfaces();

            builder.RegisterComponent(joiner)
                .As<INetworkSessionJoiner>();
            
            builder.RegisterComponent(leaver)
                .As<INetworkSessionLeaver>();

            serviceBinder.AddToModules(connector);
            serviceBinder.ListenCallbacks(connector);
        }
    }
}