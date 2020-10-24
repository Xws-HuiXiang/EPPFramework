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
}
