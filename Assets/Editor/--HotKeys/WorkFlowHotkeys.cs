// Authored by Ryan Murdoch
// Contact: rmurdoch@lucidsolutions.tech
// Copyright © Brightlobe Limited 2019

using System;
using System.ComponentModel.Design;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class WorkFlowHotkeys
{
    private static EditorWindow _mouseOverWindow;
    private static readonly object GameViewSizesInstance;
    private static readonly MethodInfo GetGroups;

    #region Hotkeys
    
    [MenuItem("Workflow/Open Loading Screen &p")]
    public static void OpenLoadingScreen()
    {
      //  EditorSceneManager.OpenScene("Assets/---Scenes/--Intro/LoadingScene.unity");
    }
    
    [MenuItem("Workflow/Toggle Lock &l")]
    private static void ToggleInspectorLock()
    {
        if (_mouseOverWindow == null)
        {
            if (!EditorPrefs.HasKey("LockableInspectorIndex"))
                EditorPrefs.SetInt("LockableInspectorIndex", 0);
            var i = EditorPrefs.GetInt("LockableInspectorIndex");

            var type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            var findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
            _mouseOverWindow = (EditorWindow)findObjectsOfTypeAll[i];
        }

        if (_mouseOverWindow == null || _mouseOverWindow.GetType().Name != "InspectorWindow") return;
        {
            var type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            var propertyInfo = type.GetProperty("isLocked");
            var value = propertyInfo != null && (bool)propertyInfo.GetValue(_mouseOverWindow, null);
            if (propertyInfo != null) propertyInfo.SetValue(_mouseOverWindow, !value, null);
            _mouseOverWindow.Repaint();
        }
    }

    [MenuItem("Workflow/Clear Console #c")]
    private static void ClearConsole()
    {
        var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        if (logEntries == null) return;
        var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
        if (clearMethod != null) clearMethod.Invoke(null, null);
    }
    
    [MenuItem("Workflow/AutoRename #r")]
    private static void AutoRename()
    {
        var objs = Selection.gameObjects;
        foreach (var obj in objs)
        {
            if (obj.GetComponent<Image>() != null)
            {
                obj.name = obj.GetComponent<Image>().sprite.name;
            }
            else if (obj.GetComponent<SpriteRenderer>() != null)
            {
                obj.name = obj.GetComponent<SpriteRenderer>().sprite.name;
            }
        }
    }
    #endregion
}

