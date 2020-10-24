using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ZIPFileWindow : EditorWindow
{
    [SerializeField]
    private string folder;

    [MenuItem("EPP Tools/ZIP File Window")]
    public static void OpenWindow()
    {
        ZIPFileWindow window = EditorWindow.GetWindow<ZIPFileWindow>("ZIP文件处理");

        window.Show();
    }

    private void OnGUI()
    {
        folder = EditorGUILayout.TextField("要压缩的文件夹", folder);

        if (GUILayout.Button("创建ZIP文件"))
        {
            //ZIPFileUtil.CreateZIPFile()
        }
    }
}
