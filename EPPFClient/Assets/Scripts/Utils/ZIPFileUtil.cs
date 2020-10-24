using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

public static class ZIPFileUtil
{
    /// <summary>
    /// 创建ZIP压缩文件。压缩文件中不包含compressionFolder这个文件夹
    /// </summary>
    /// <param name="zipFileFullName">zip文件的完整路径</param>
    /// <param name="compressionFolder">要压缩的文件夹完整路径</param>
    /// <param name="zipPassword">zip包密码</param>
    /// <param name="zipComment">zip包备注</param>
    /// <returns>创建成功返回true，创建失败返回false</returns>
    public static bool CreateZIPFile(string zipFileFullName, string compressionFolder, string zipPassword, string zipComment)
    {
        FileStream fsOut = null;
        ZipFile zipFile = null;
        FileStream streamReader = null;
        bool res = false;
        try
        {
            fsOut = File.Create(zipFileFullName);
            zipFile = ZipFile.Create(fsOut);
            zipFile.BeginUpdate();

            //准备压缩到文件夹中的目录
            DirectoryInfo directoryInfo = new DirectoryInfo(compressionFolder);

            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                //不打包.meta文件和以.开头的文件或文件夹
                if(files[i].Name.EndsWith(".meta") || files[i].Name.StartsWith("."))
                {
                    continue;
                }

                //去掉文件根目录 和最后的分隔符
                string compressionFolderNew = compressionFolder.Replace("/", "\\");
                string zipEntryName = files[i].FullName.Replace(compressionFolderNew, "").Substring(1);
                //ZipEntry代表了压缩包中的一个项 可以是文件 也可以是文件夹
                ZipEntry entry = new ZipEntry(zipEntryName);

                //解决中文乱码问题。使用UTF8编码
                entry.IsUnicodeText = true;

                //将要压缩的项添加到zip流中
                byte[] buffer = new byte[4096];
                streamReader = File.OpenRead(files[i].FullName);
                CustomStaticDataSource dataSource = new CustomStaticDataSource();
                dataSource.SetStream(streamReader);

                zipFile.Add(dataSource, entry);
            }

            //压缩文件的信息内容
            zipFile.SetComment(zipComment);
            //压缩文件密码
            zipFile.Password = zipPassword;

            zipFile.CommitUpdate();
            res = true;

            FDebugger.LogFormat("zip文件[{0}]创建完成", zipFileFullName);
        }
        catch (Exception e)
        {
            res = false;

            FDebugger.LogError("创建zip文件失败。异常信息：" + e.Message);
        }
        finally
        {
            if (fsOut != null)
            {
                fsOut.Close();
            }
            if (zipFile != null)
            {
                zipFile.Close();
            }
            if (streamReader != null)
            {
                streamReader.Close();
            }
        }

        return res;
    }

    /// <summary>
    /// 解压缩zip文件
    /// </summary>
    /// <param name="fileFullPath">zip压缩文件完整路径</param>
    /// <param name="targetFolder">解压缩目录的完整路径</param>
    /// <param name="password">压缩文件密码</param>
    public static void UnzipFile(string fileFullPath, string targetFolder, string password)
    {
        try
        {
            FastZip fastZip = new FastZip();
            fastZip.Password = password;
            fastZip.ExtractZip(fileFullPath, targetFolder, FastZip.Overwrite.Always, null, null, null, true, false);
            
            FDebugger.LogFormat("文件[{0}]解压完成", fileFullPath);
        }
        catch(Exception e)
        {
            FDebugger.LogErrorFormat("解压缩文件[{0}]出现异常。异常信息：" + e.Message, fileFullPath);
        }
    }

    /// <summary>
    /// 解压缩zip文件。返回byte数组不创建文件
    /// </summary>
    /// <param name="fileFullPath">zip压缩文件完整路径</param>
    /// <param name="contentList">文件内容列表</param>
    /// <param name="password">压缩文件密码</param>
    public static void UnzipFile(string fileFullPath, out List<byte[]> contentList, string password)
    {
        contentList = new List<byte[]>();

        FileStream fileStream = File.Open(fileFullPath, FileMode.Open);
        using (ZipInputStream zipInputStream = new ZipInputStream(fileStream))
        {
            zipInputStream.Password = password;
            ZipEntry entry = zipInputStream.GetNextEntry();
            while (entry != null)
            {
                string entryFileName = entry.Name;
                byte[] buffer = new byte[4096];

                //跳过目录
                string fullZipToPath = Path.Combine(UnityEngine.Application.dataPath, entryFileName);
                if (Path.GetFileName(fullZipToPath).Length == 0)
                {
                    entry = zipInputStream.GetNextEntry();

                    continue;
                }

                using (MemoryStream streamWriter = new MemoryStream())
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                    //StreamUtils.ReadFully(zipInputStream, buffer);
                    contentList.Add(streamWriter.ToArray());
                }

                entry = zipInputStream.GetNextEntry();
            }
        }
        fileStream.Close();
    }

    /// <summary>
    /// 解压缩zip文件。解压ab包的压缩文件，不解压manifest文件
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <param name="contentList"></param>
    /// <param name="password"></param>
    public static void UnzipABFile(string fileFullPath, out List<byte[]> contentList, string password)
    {
        contentList = new List<byte[]>();

        FileStream fileStream = File.Open(fileFullPath, FileMode.Open);
        using (ZipInputStream zipInputStream = new ZipInputStream(fileStream))
        {
            zipInputStream.Password = password;
            ZipEntry entry = zipInputStream.GetNextEntry();
            while (entry != null)
            {
                string entryFileName = entry.Name;
                if (entryFileName.EndsWith(".manifest"))
                {
                    entry = zipInputStream.GetNextEntry();
                    continue;
                }

                //跳过目录
                string fullZipToPath = Path.Combine(UnityEngine.Application.dataPath, entryFileName);
                if (Path.GetFileName(fullZipToPath).Length == 0)
                {
                    entry = zipInputStream.GetNextEntry();
                    continue;
                }

                byte[] buffer = new byte[4096];

                using (MemoryStream streamWriter = new MemoryStream())
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                    //StreamUtils.ReadFully(zipInputStream, buffer);
                    contentList.Add(streamWriter.ToArray());
                }

                entry = zipInputStream.GetNextEntry();
            }
        }
        fileStream.Close();
    }

    /* 官网例子
    //https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples
    public static void UnzipFromStream(Stream zipStream, string outFolder)
    {
        using (var zipInputStream = new ZipInputStream(zipStream))
        {
            while (zipInputStream.GetNextEntry() is ZipEntry zipEntry)
            {
                var entryFileName = zipEntry.Name;
                // To remove the folder from the entry:
                //var entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here
                // to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                // 4K is optimum
                var buffer = new byte[4096];

                // Manipulate the output filename here as desired.
                var fullZipToPath = Path.Combine(outFolder, entryFileName);
                var directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Skip directory entry
                if (Path.GetFileName(fullZipToPath).Length == 0)
                {
                    continue;
                }

                // Unzip file in buffered chunks. This is just as fast as unpacking
                // to a buffer the full size of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                }
            }
        }
    }
    */

    /// <summary>
    /// 创建zip压缩文件时使用的数据源
    /// </summary>
    public class CustomStaticDataSource : IStaticDataSource
    {
        private Stream stream;
        /// <summary>
        /// 获取设置的数据流
        /// </summary>
        /// <returns></returns>
        public Stream GetSource()
        {
            return stream;
        }

        /// <summary>
        /// 调用这个方法用于提供数据流
        /// </summary>
        /// <param name="inputStream"></param>
        public void SetStream(Stream inputStream)
        {
            stream = inputStream;
            stream.Position = 0;
        }
    }
}
