using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using System.IO;
using System;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;

public class ShareZipLibTest : EditorWindow
{
    [MenuItem("EPP Tools/创建zip文件")]
    public static void ClickMenu()
    {
        ZIPFileUtil.CreateZIPFile(@"E:\压缩测试\保存zip\TestZip.zip", @"E:\压缩测试\根目录\准备压缩的文件", "mima", null);
    }

    [MenuItem("EPP Tools/解压缩zip文件")]
    public static void ClickMenu2()
    {
        ZIPFileUtil.UnzipFile(@"E:\压缩测试\保存zip\TestZip.zip", @"E:\压缩测试\解压缩目录", "mima");
    }

    [MenuItem("EPP Tools/DH算法测试")]
    public static void DiffieHellman()
    {
        ShareZipLibTest window = EditorWindow.GetWindow<ShareZipLibTest>("测试DH算法");
    }

    private int a, p, xa, xb;

    private void OnGUI()
    {
        a = EditorGUILayout.IntField("a 生成元", a);
        p = EditorGUILayout.IntField("p 模数", p);
        xa = EditorGUILayout.IntField("xa  Alice输入自己的秘密数", xa);
        xb = EditorGUILayout.IntField("xb Bob输入自己的秘密数", xb);

        if (GUILayout.Button("生成"))
        {
            int ya = Mod(a, xa, p);
            int yb = Mod(a, xb, p);

            Debug.Log("Alice的公钥为：" + ya);
            Debug.Log("Bob的公钥为：" + yb);

            int ka = Mod(yb, xa, p);
            int kb = Mod(ya, xb, p);

            Debug.Log("Alice和Bob两人之间的共享密钥为Ka:" + ka + "    或者Kb:" + kb);
        }
    }

    private int Mod(int x, int y, int z)
    {
        int s = (int)Mathf.Pow(x, y);
        return s % z;
    }
}
