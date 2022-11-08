﻿using Common.ReadOnlyDictionaries.Editor;
using GamePlay.Common.SceneObjects.Logs;
using UnityEditor;

namespace Global.Services.Loggers.Editor.Services
{
    [CustomPropertyDrawer(typeof(SceneObjectLogs))]
    public class SceneObjectLogsDrawer : ReadonlyDictionaryPropertyDrawer
    {
        protected override bool IsCollapsed => false;
    }
}