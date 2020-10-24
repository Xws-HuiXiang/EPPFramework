using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace EPPTools.Utils
{
    public class CreateOtherFileUtils
    {
        private const string MenuPath = "EPP Tools/Create File/";

        private const string MarkDownFileName = "MarkDowm File.md";
        private const string TxtFileName = "Txt File.txt";
        private const string JsonFileName = "Json File.json";
        private const string XMLFileName = "XML File.xml";
        private const string LangFileName = "Lang File.lang";
        private const string LuaFileName = "Lua File.lua";

        [MenuItem(MenuPath + "Mark Dowm", priority = 1)]
        public static void CreateMarkDownFile()
        {
            TryCreateFile(GetFilePath(MarkDownFileName));
        }

        [MenuItem(MenuPath + "TXT", priority = 2)]
        public static void CreateTXTFile()
        {
            TryCreateFile(GetFilePath(TxtFileName));
        }

        [MenuItem(MenuPath + "Json", priority = 3)]
        public static void CreateJsonFile()
        {
            TryCreateFile(GetFilePath(JsonFileName));
        }

        [MenuItem(MenuPath + "XML", priority = 4)]
        public static void CreateXMLFile()
        {
            TryCreateFile(GetFilePath(XMLFileName));
        }

        [MenuItem(MenuPath + "Lang", priority = 5)]
        public static void CreateLangFile()
        {
            TryCreateFile(GetFilePath(LangFileName));
        }

        [MenuItem(MenuPath + "Lua", priority = 6)]
        public static void CreateLuaFile()
        {
            TryCreateFile(GetFilePath(LuaFileName));
        }

        /// <summary>
        /// 创建指定名字的文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateFile(string fileName)
        {
            TryCreateFile(GetFilePath(fileName));
        }

        /// <summary>
        /// 根据当前在编辑器中选择的物体返回文件的完整路径
        /// </summary>
        /// <returns></returns>
        private static string GetFilePath(string fileName)
        {
            string filePath = "";
            if (Selection.activeObject != null)
            {
                //当前选择了一个文件或文件夹
                filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
                //区分是选择的Hierarchy还是Project面板的物体
                if (filePath == "")
                {
                    //选择的是Hierarchy面板物体
                    filePath = Application.dataPath + "/" + fileName;
                }
                else
                {
                    //选择的是Project面板物体
                    int pointIndex = filePath.LastIndexOf('.');
                    if (pointIndex != -1)
                    {
                        //选择了文件
                        string subPath = filePath.Substring(6);
                        int index = subPath.LastIndexOf('/');
                        subPath = subPath.Substring(0, index + 1);
                        filePath = Application.dataPath + subPath + "/" + fileName;
                    }
                    else
                    {
                        //选择了文件夹
                        filePath = Application.dataPath + filePath.Substring(6) + "/" + fileName;
                    }
                }
            }
            else
            {
                //当前没有选择物体，创建在Assets目录下
                filePath = Application.dataPath + "/" + fileName;
            }

            return filePath;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        private static void TryCreateFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                DateTime time = DateTime.Now;
                string timeName = (time.Year + time.Month + time.Day + time.Hour + time.Minute + time.Second).ToString();
                string[] split = filePath.Split('.');
                filePath = split[0] + timeName + "." + split[1];
            }

            FileStream fs = File.Create(filePath);
            fs.Close();

            AssetDatabase.Refresh();

            int startIndex = filePath.IndexOf("Assets");
            string projectPath = filePath.Substring(startIndex);
            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(projectPath);
            Selection.activeObject = o;
        }
    }
}
