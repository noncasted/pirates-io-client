﻿using Global.Common;
using Global.Services.Loggers.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Global.Services.Network.Session.Join.Logs
{
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [CreateAssetMenu(fileName = GlobalAssetsPaths.LogsPrefix + "NetworkSessionJoin",
        menuName = GlobalAssetsPaths.NetworkSession + "JoinLogs")]
    public class NetworkSessionJoinLogSettings : LogSettings<NetworkSessionJoinLogs, NetworkSessionLogType>
    {
        [SerializeField] [Indent] private LogParameters _logParameters;

        public LogParameters LogParameters => _logParameters;
    }
}