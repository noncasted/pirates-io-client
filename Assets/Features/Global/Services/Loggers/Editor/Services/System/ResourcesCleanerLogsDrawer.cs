﻿using Common.ReadOnlyDictionaries.Editor;
using Global.Services.ResourcesCleaners.Logs;
using UnityEditor;

namespace Global.Services.Loggers.Editor.Services.System
{
    [CustomPropertyDrawer(typeof(ResourcesCleanerLogs))]
    public class ResourcesCleanerLogsDrawer : ReadonlyDictionaryPropertyDrawer
    {
        protected override bool IsCollapsed => false;
    }
}