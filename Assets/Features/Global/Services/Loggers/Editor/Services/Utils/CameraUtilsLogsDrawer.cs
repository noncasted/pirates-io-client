﻿using Common.ReadOnlyDictionaries.Editor;
using Global.Services.CameraUtilities.Logs;
using UnityEditor;

namespace Global.Services.Loggers.Editor.Services.Cameras
{
    [ReadOnlyDictionaryPriority]
    [CustomPropertyDrawer(typeof(CameraUtilsLogs))]
    public class CameraUtilsLogsDrawer : ReadonlyDictionaryPropertyDrawer
    {
        protected override bool IsCollapsed => false;
    }
}