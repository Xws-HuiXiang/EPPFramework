using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EPPTools.PluginSettings;
using System;
using EPPTools.Utils;
using Ciphertext;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EPPTools.CreateAssetsBundleTools
{
    [Serializable]
    public class CreateAssetsBundleWindow : EditorWindow
    {
        /// <summary>
        /// 资源包打包的资源范围
        /// </summary>
        [SerializeField]
        private static BuildBundleFunction buildBundleFunction = BuildBundleFunction.TargetFolder;

        /// <summary>
        /// TargetFolder模式下的资源根文件夹
        /// </summary>
        [SerializeField]
        private static string resTargetFolderRootFolder;
        /// <summary>
        /// TargetFolder模式下的lua代码根文件夹
        /// </summary>
        [SerializeField]
        private static string luaTargetFolderRootFolder;
        /// <summary>
        /// TargetFolder模式下的Tolua代码根文件夹。ToLua框架提供的基础lua脚本目录
        /// </summary>
        [SerializeField]
        private static string toLuaTargetFolderRootFolder;
        /// <summary>
        /// 是否打包资源
        /// </summary>
        [SerializeField]
        private static bool packageRes;
        /// <summary>
        /// 是否打包lua代码
        /// </summary>
        [SerializeField]
        private static bool packageLuaCode;
        /// <summary>
        /// lua文件输出路径
        /// </summary>
        [SerializeField]
        private static string luaOutPutPath;
        /// <summary>
        /// 是否生成zip文件
        /// </summary>
        //[SerializeField]
        //private static bool generateZipFile;
        /// <summary>
        /// zip压缩文件密码
        /// </summary>
        //[SerializeField]
        //private static string zipFilePassword;
        /// <summary>
        /// zip文件保存路径
        /// </summary>
        //[SerializeField]
        //private static string zipOutPutPath;

        /// <summary>
        /// 统一bundle的名称
        /// </summary>
        [SerializeField]
        private static string bundlesName;
        /// <summary>
        /// 打包前自动修改包名
        /// </summary>
        [SerializeField]
        private static bool autoChangeAssetBundleName;

        /// <summary>
        /// AB包输出路径
        /// </summary>
        [SerializeField]
        private static string outPutPath;
        /// <summary>
        /// ab包压缩设置
        /// </summary>
        [SerializeField]
        private static BuildAssetBundleOptions buildAssetBundleOptions;
        /// <summary>
        /// ab包目标平台
        /// </summary>
        [SerializeField]
        private static BuildTarget buildTarget;
        /// <summary>
        /// 后缀名
        /// </summary>
        [SerializeField]
        private static string suffixName;
        /// <summary>
        /// 热更新资源版本
        /// </summary>
        //private static int resHotfixVersion;
        /// <summary>
        /// 热更新代码版本
        /// </summary>
        //private static int luaHotfixVersion;

        [MenuItem("EPP Tools/Create Assets Bundle")]
        public static void OpenWindow()
        {
            CreateAssetsBundleWindow window = EditorWindow.GetWindow<CreateAssetsBundleWindow>("Assets Bundle工具");

            buildBundleFunction = (BuildBundleFunction)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildBundleFunction;
            resTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.targetFolderRootFolder;
            bundlesName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.bundlesName;
            autoChangeAssetBundleName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.autoChangeAssetBundleName;
            outPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.outPutPath;
            buildAssetBundleOptions = (BuildAssetBundleOptions)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildAssetBundleOptions;
            buildTarget = (BuildTarget)EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.buildTarget;
            suffixName = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.suffixName;
            luaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaTargetFolderRootFolder;
            //toLuaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.toLuaTargetFolderRootFolder;
            packageRes = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.packageRes;
            packageLuaCode = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.packageLuaCode;
            //generateZipFile = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.generateZipFile;
            //zipFileSavePath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipOutPutPath;
            toLuaTargetFolderRootFolder = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.toLuaTargetFolderRootFolder;
            //zipFilePassword = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipFilePassword;
            //zipOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.zipOutPutPath;
            luaOutPutPath = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaOutPutPath;
            //resHotfixVersion = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.resHotfixVersion;
            //luaHotfixVersion = EPPToolsSettingAsset.Instance.CreateAssetsBundleConfig.luaHotfixVersion;

            window.Show();
        }

        private void OnGUI()
        {
            buildBundleFunction = (BuildBundleFunction)EditorGUILayout.EnumPopup("资源打包范围", buildBundleFunction);
            switch (buildBundleFunction)
            {
                case BuildBundleFunction.AllAssets:

                    break;
                case BuildBundleFunction.TargetFolder:
                    PickUpPathButtonGUI("根目录", ref resTargetFolderRootFolder);
                    EditorGUILayout.BeginHorizontal();
                    bundlesName = EditorGUILayout.TextField("统一包名称为", bundlesName);
                    if (GUILayout.Button("修改", GUILayout.MaxWidth(60f)))
                    {
                        if (!string.IsNullOrEmpty(bundlesName))
                        {
                            ClearAllAssetsBundleName();
                            SetAssetBundleName(resTargetFolderRootFolder, bundlesName);
                        }
                    }
                    autoChangeAssetBundleName = EditorGUILayout.ToggleLeft("自动修改包名", autoChangeAssetBundleName, GUILayout.MaxWidth(100));
                    EditorGUILayout.EndHorizontal();
                    break;
                case BuildBundleFunction.DistributedFolder:
                    PickUpPathButtonGUI("资源根目录", ref resTargetFolderRootFolder);
                    PickUpPathButtonGUI("Lua代码根目录", ref luaTargetFolderRootFolder);
                    PickUpPathButtonGUI("ToLua代码根目录", ref toLuaTargetFolderRootFolder);
                    packageRes = EditorGUILayout.ToggleLeft("打包资源", packageRes);
                    packageLuaCode = EditorGUILayout.ToggleLeft("打包Lua代码", packageLuaCode);
                    if (packageLuaCode)
                    {
                        PickUpPathButtonGUI("lua文件保存路径", ref luaOutPutPath);
                    }
                    //generateZipFile = EditorGUILayout.ToggleLeft("生成zip文件", generateZipFile);
                    //if (generateZipFile)
                    //{
                        //zipFilePassword = EditorGUILayout.TextField("zip文件密码", zipFilePassword);
                        //PickUpPathButtonGUI("zip文件保存路径", ref zipOutPutPath);
                    //}
                    break;
            }
            PickUpPathButtonGUI("ab包保存路径", ref outPutPath);
            //resHotfixVersion = EditorGUILayout.IntField("热更新资源版本", resHotfixVersion);
            //luaHotfixVersion = EditorGUILayout.IntField("热更新代码版本", luaHotfixVersion);

            suffixName = EditorGUILayout.TextField("后缀名", suffixName);
            buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("压缩方式", buildAssetBundleOptions);
            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("目标平台", buildTarget);

            if (GUILayout.Button("打包"))
            {
                List<string> ignoreFolderList = new List<string>() { ".idea" };
                List<string> ignoreFileList = new List<string>() { ".meta" };

                //打包资源
                if (packageRes)
                {
                    //清理文件夹
                    ClearFolder(Path.Combine(Application.dataPath, outPutPath.Substring(6)));
                    //单独处理分文件夹打包的情况
                    if (buildBundleFunction == BuildBundleFunction.DistributedFolder)
                    {
                        //重新设置目标文件夹下的所有资源的ab包名称
                        ResetAssetsBundleName(resTargetFolderRootFolder, null);

                        BuildPipeline.BuildAssetBundles(outPutPath, buildAssetBundleOptions, buildTarget);
                    }
                    else
                    {
                        //当选择为目标文件夹模式时统一修改包名称
                        if (buildBundleFunction == BuildBundleFunction.TargetFolder && autoChangeAssetBundleName)
                        {
                            SetAssetBundleName(resTargetFolderRootFolder, bundlesName);
                        }

                        BuildPipeline.BuildAssetBundles(outPutPath, buildAssetBundleOptions, buildTarget);
                    }
                }

                //Res部分
                PackageRes(ignoreFileList);
                //Lua代码部分
                PackageLua(ignoreFileList, ignoreFolderList);

                SaveConfig();

                AssetDatabase.Refresh();

                Debug.Log("资源打包完成");
            }
        }

        /// <summary>
        /// 重新设置所有资源的ab名称
        /// </summary>
        /// <param name="rootFolder">打包时的ab包根路径</param>
        /// <param name="abName">ab包的名称。如果为null则使用资源所在的文件夹名称</param>
        private void ResetAssetsBundleName(string rootFolder, string abName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, rootFolder.Substring(7)));
            DirectoryInfo[] childDirectoryInfos = directoryInfo.GetDirectories();

            //清理包名称
            ClearAllAssetsBundleName();

            //设置所有的资源名称
            for (int i = 0; i < childDirectoryInfos.Length; i++)
            {
                //如果不指定ab名称则使用文件夹名称作为ab包的名称
                string bundleName = (abName != null) ? abName : childDirectoryInfos[i].Name;

                if (!string.IsNullOrEmpty(suffixName))
                {
                    char firstChar = suffixName[0];
                    if (firstChar != '.')
                    {
                        bundleName += ".";
                    }
                    bundleName += suffixName;
                }
                SetAssetBundleName(Path.Combine(rootFolder, childDirectoryInfos[i].Name), bundleName);
            }
        }

        /// <summary>
        /// 设置目标文件夹下的所有文件的AssetBundle名称
        /// </summary>
        /// <param name="rootFolderPath"></param>
        /// <param name="bundlesName"></param>
        private void SetAssetBundleName(string rootFolderPath, string bundlesName)
        {
            //名称分隔
            string[] arr = bundlesName.Split('.');

            //遍历文件夹下的所有文件并设置包名
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, rootFolderPath.Substring(7)));
            //获取所有文件
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            List<string> filePathList = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                //处理文件路径为unity工程内的局部路径
                string fileFullName = files[i].FullName;
                int startIndex = fileFullName.IndexOf("Assets" + Path.DirectorySeparatorChar);
                string localPath = "";
                if (startIndex != -1)
                {
                    localPath = fileFullName.Substring(startIndex, fileFullName.Length - startIndex);
                }
                filePathList.Add(localPath);
            }
            foreach (string path in filePathList)
            {
                //剔除meta文件
                if (!path.EndsWith(".meta"))
                {
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if(importer != null)
                    {
                        importer.assetBundleName = arr[0];
                        if (arr.Length > 1)
                        {
                            importer.assetBundleVariant = arr[1];
                        }
                    }
                    else
                    {
                        Debug.LogWarningFormat("importer为空，资源路径为：{0}", path);
                    }
                }
            }
        }

        /// <summary>
        /// 清理所有的资源ab包名称
        /// </summary>
        private void ClearAllAssetsBundleName()
        {
            //清理所有的ab包名称
            string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < allAssetBundleNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(allAssetBundleNames[i], true);
            }
        }

        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="dir"></param>
        private void ClearFolder(string dir)
        {
            if (!Directory.Exists(dir)) return;

            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件 
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        ClearFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }

        /// <summary>
        /// 绘制拾取路径按钮
        /// </summary>
        /// <param name="outPutString"></param>
        private void PickUpPathButtonGUI(string fieldTitle, ref string outPutString)
        {
            EditorGUILayout.BeginHorizontal();
            outPutString = EditorGUILayout.TextField(fieldTitle, outPutString);
            if (GUILayout.Button("拾取", GUILayout.MaxWidth(60f)))
            {
                if (Selection.assetGUIDs.Length > 0)
                {
                    string selectedGUID = Selection.assetGUIDs[0];
                    string assetsPath = AssetDatabase.GUIDToAssetPath(selectedGUID);
                    if (assetsPath.Contains("."))
                    {
                        //是一个文件，选择当前文件所在的文件夹
                        int lastSlice = assetsPath.LastIndexOf('/');
                        outPutString = assetsPath.Substring(0, lastSlice);
                    }
                    else
                    {
                        //选了一个文件夹，直接赋值路径
                        outPutString = AssetDatabase.GUIDToAssetPath(selectedGUID);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            CreateAssetsBundleConfig config = new CreateAssetsBundleConfig();

            config.buildBundleFunction = (int)buildBundleFunction;
            config.targetFolderRootFolder = resTargetFolderRootFolder;
            config.bundlesName = "空";
            config.autoChangeAssetBundleName = autoChangeAssetBundleName;
            config.outPutPath = outPutPath;
            config.buildAssetBundleOptions = (int)buildAssetBundleOptions;
            config.buildTarget = (int)buildTarget;
            config.suffixName = suffixName;
            config.luaTargetFolderRootFolder = luaTargetFolderRootFolder;
            config.toLuaTargetFolderRootFolder = toLuaTargetFolderRootFolder;
            config.packageRes = packageRes;
            config.packageLuaCode = packageLuaCode;
            //config.generateZipFile = generateZipFile;
            //config.zipOutPutPath = zipOutPutPath;
            config.luaOutPutPath = luaOutPutPath;
            //config.zipFilePassword = zipFilePassword;
            //config.resHotfixVersion = resHotfixVersion;
            //config.luaHotfixVersion = luaHotfixVersion;

            EPPToolsSettingAsset.Instance.SetCreateAssetsBundleConfig(config);
        }

        /// <summary>
        /// res打包相关的逻辑
        /// </summary>
        /// <param name="ignoreFileList"></param>
        private void PackageRes(List<string> ignoreFileList)
        {
            //对ab包使用AES加密处理。先复制一份出来用于加密
            string resFullPath = Path.Combine(Application.dataPath, outPutPath.Substring(7));
            string targetFullPath = Path.Combine(Application.dataPath, "AssetBundle", "AESEncryption", "Res");
            DirectoryUtils.CopyDirectory(resFullPath, targetFullPath, ignoreFileList, null);
            //不加密AssetBundle文件
            string[] targetFolderFiles = Directory.GetFiles(targetFullPath, "*.*", SearchOption.AllDirectories).Where(
                s => s.EndsWith(AppConst.AssetBundleSuffix) ||
                     s.EndsWith(".manifest")).ToArray();

            for (int i = 0; i < targetFolderFiles.Length; i++)
            {
                string fileFullName = targetFolderFiles[i];
                string fileName = Path.GetFileName(fileFullName);
                fileName = fileName.Replace(AppConst.AssetBundleSuffix, "");//去掉.ab
                string directoryName = Path.GetDirectoryName(fileFullName);
                string encryptionFileFullPath = Path.Combine(directoryName, fileName + AppConst.EncryptionFillSuffix);

                byte[] fileBytes = File.ReadAllBytes(fileFullName);
                byte[] encryptionBytes = AES.AESEncrypt(fileBytes, AppConst.AbPackageKey);

                if (File.Exists(encryptionFileFullPath))
                {
                    File.Delete(encryptionFileFullPath);
                }
                if (File.Exists(fileFullName))
                {
                    File.Delete(fileFullName);
                }
                FileStream fs = File.Create(encryptionFileFullPath);
                fs.Write(encryptionBytes, 0, encryptionBytes.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// lua打包相关逻辑
        /// </summary>
        /// <param name="ignoreFileList"></param>
        /// <param name="ignoreFolderList"></param>
        private void PackageLua(List<string> ignoreFileList, List<string> ignoreFolderList)
        {
            if (packageLuaCode)
            {
                //lua代码。不打成ab包，直接将文件复制到目标目录
                if (!Directory.Exists(luaOutPutPath))
                {
                    Directory.CreateDirectory(luaOutPutPath);
                }
                //复制项目的业务代码
                DirectoryUtils.CopyDirectory(luaTargetFolderRootFolder, luaOutPutPath, ignoreFileList, ignoreFolderList, true);
                //复制ToLua框架代码
                DirectoryUtils.CopyDirectory(toLuaTargetFolderRootFolder, luaOutPutPath, ignoreFileList, ignoreFolderList, true);
            }

            //使用AES加密
            string[] luaFilesFullPath = Directory.GetFiles(luaOutPutPath, "*.lua", SearchOption.AllDirectories);
            for (int i = 0; i < luaFilesFullPath.Length; i++)
            {
                byte[] data = File.ReadAllBytes(luaFilesFullPath[i]);
                data = AES.AESEncrypt(data, AppConst.AbPackageKey);

                File.Delete(luaFilesFullPath[i]);
                string newFileFullPath = luaFilesFullPath[i].Substring(0, luaFilesFullPath[i].Length - 4) + AppConst.EncryptionFillSuffix;
                FileStream fs = File.Create(newFileFullPath);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// MD5值检测，如果MD5值相同则删除上一个版本的文件
        /// </summary>
        /// <param name="outAssetPath"></param>
        /// <param name="hotfixVersion"></param>
        /// <param name="folderName"></param>
        [Obsolete("不在Unity中处理MD5值校验与版本管理，需要使用提供的额外工具进行版本控制")]
        private void CheckMD5(string outAssetPath, int hotfixVersion, string folderName)
        {
            //对比之前版本的文件，决定是否剔除。版本号大于1才有之前的版本文件信息
            if (hotfixVersion > 1)
            {
                int lowHotfixVersion = hotfixVersion - 1;
                string lowResPath = Path.Combine(outAssetPath.Substring(0, outAssetPath.Length - 6), lowHotfixVersion.ToString(), folderName + lowHotfixVersion.ToString());
                string[] heightVersionFiles = Directory.GetFiles(outAssetPath, "*.*", SearchOption.AllDirectories);
                string[] lowVersionFiles = Directory.GetFiles(lowResPath, "*.*", SearchOption.AllDirectories);

                List<string> lowVersionFileNamesList = new List<string>();
                for (int i = 0; i < lowVersionFiles.Length; i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(lowVersionFiles[i]);
                    lowVersionFileNamesList.Add(fileName);
                }
                for (int i = 0; i < heightVersionFiles.Length; i++)
                {
                    string heightVersionFileFullPath = heightVersionFiles[i];
                    string fileName = Path.GetFileNameWithoutExtension(heightVersionFileFullPath);
                    if (lowVersionFileNamesList.Contains(fileName))
                    {
                        //以前的版本也包含这个文件，校验两个文件的md5值
                        string heightMD5 = GetMD5HashFromFile(heightVersionFiles[i]);
                        int lowVersionFileIndex = lowVersionFileNamesList.IndexOf(fileName);
                        string lowMD5 = GetMD5HashFromFile(lowVersionFiles[lowVersionFileIndex]);
                        //Debug.LogFormat("校验两个MD5：高版本[{0}]，低版本[{1}]", heightMD5, lowMD5);

                        if (heightMD5.Equals(lowMD5))
                        {
                            //md5值相同，说明文件没有改动，删除该文件
                            File.Delete(heightVersionFileFullPath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件的MD5码
        /// </summary>
        /// <param name="fileName">传入的文件名（含路径及后缀名）</param>
        /// <returns></returns>
        public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
