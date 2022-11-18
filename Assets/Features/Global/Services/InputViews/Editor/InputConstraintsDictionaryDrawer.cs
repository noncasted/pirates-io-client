﻿using Common.ReadOnlyDictionaries.Editor;
using Features.Global.Services.InputViews.ConstraintsStorage;
using UnityEditor;

namespace Global.Services.InputViews.Editor
{
    [CustomPropertyDrawer(typeof(InputConstraintsDictionary))]
    public class InputConstraintsDictionaryDrawer : ReadonlyDictionaryPropertyDrawer
    {
        protected override bool IsCollapsed => false;
    }
}