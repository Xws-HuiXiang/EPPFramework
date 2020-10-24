using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EPPTools.Utils
{
    public class DirectoryUtils : MonoBehaviour
    {
        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="sourceDirPath">源文件夹</param>
        /// <param name="targetDirPath">目标文件夹</param>
        /// <param name="overwriteFile">是否重写文件</param>
        public static void CopyDirectory(string sourceDirPath, string targetDirPath, bool overwriteFile = true)
        {
            /*
            try
            {
                //如果指定的存储路径不存在，则创建该存储路径
                if (!Directory.Exists(targetDirPath))
                {
                    //创建
                    Directory.CreateDirectory(targetDirPath);
                }
                //获取源路径文件的名称
                string[] files = Directory.GetFiles(sourceDirPath);
                //遍历子文件夹的所有文件
                foreach (string file in files)
                {
                    string pFilePath = targetDirPath + "\\" + Path.GetFileName(file);
                    //if (File.Exists(pFilePath))
                    //    continue;
                    File.Copy(file, pFilePath, overwriteFile);
                }
                string[] dirs = Directory.GetDirectories(sourceDirPath);
                //递归，遍历文件夹
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, targetDirPath + "\\" + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            */
            CopyDirectory(sourceDirPath, targetDirPath, null, overwriteFile);
        }

        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="sourceDirPath">源文件夹</param>
        /// <param name="targetDirPath">目标文件夹</param>
        /// <param name="ignoreSuffix">忽略文件的规则。不拷贝文件名称以某些字符串结尾的文件</param>
        /// <param name="overwriteFile">是否重写文件</param>
        public static void CopyDirectory(string sourceDirPath, string targetDirPath, List<string> ignoreSuffix, bool overwriteFile = true)
        {
            try
            {
                //如果指定的存储路径不存在，则创建该存储路径
                if (!Directory.Exists(targetDirPath))
                {
                    //创建
                    Directory.CreateDirectory(targetDirPath);
                }
                //获取源路径文件的名称
                string[] files = Directory.GetFiles(sourceDirPath);
                //遍历子文件夹的所有文件
                foreach (string file in files)
                {
                    //如果忽略列表中包含这个文件的扩展名，则跳过
                    if(ignoreSuffix != null)
                    {
                        string suffixName = Path.GetExtension(file);
                        if (ignoreSuffix.Contains(suffixName))
                        {
                            continue;
                        }
                    }

                    string pFilePath = targetDirPath + "\\" + Path.GetFileName(file);
                    //if (File.Exists(pFilePath))
                    //    continue;
                    File.Copy(file, pFilePath, overwriteFile);
                }
                string[] dirs = Directory.GetDirectories(sourceDirPath);
                //递归，遍历文件夹
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, targetDirPath + "\\" + Path.GetFileName(dir), ignoreSuffix);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
