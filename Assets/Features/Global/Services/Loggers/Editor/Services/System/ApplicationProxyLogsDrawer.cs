﻿#region

using Common.ReadOnlyDictionaries.Editor;
using Global.Services.ApplicationProxies.Logs;
using UnityEditor;

#endregion

namespace Global.Services.Loggers.Editor.Services.System
{
    [CustomPropertyDrawer(typeof(ApplicationProxyLogs))]
    public class ApplicationProxyLogsDrawer : ReadonlyDictionaryPropertyDrawer
    {
        protected override bool IsCollapsed => false;
    }
}