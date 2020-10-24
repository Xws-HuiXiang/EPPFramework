using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EntranceScene
{
    static EntranceScene()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChangedAction;
    }

    private static void PlayModeStateChangedAction(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                break;
            case PlayModeStateChange.ExitingEditMode:
                Scene firstScene = SceneManager.GetSceneAt(0);
                if (firstScene != null)
                {
                    SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/FirstScene.unity");
                    EditorSceneManager.playModeStartScene = scene;
                }
                break;
            case PlayModeStateChange.EnteredPlayMode:
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;

        }
    }
}
