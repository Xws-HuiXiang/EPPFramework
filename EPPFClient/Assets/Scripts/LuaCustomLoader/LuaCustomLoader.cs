using Ciphertext;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 自定义lua加载器。因为lua文件是加密过的，所以自定义加载方式
/// </summary>
public class LuaCustomLoader : LuaFileUtils
{
    public LuaCustomLoader()
    {
        instance = this;
        beZip = false;
    }

    public override byte[] ReadFile(string fileName)
    {
        if(!string.IsNullOrEmpty(fileName))
        {
            string filePath = FindFile(fileName);
            byte[] fileData;
            if(filePath != null)
            {
                if (filePath.EndsWith(AppConst.EncryptionFillSuffix))
                {
                    //是自定义加密的文件，读取文件后解密出内容
                    byte[] encryptionData = File.ReadAllBytes(filePath);
                    fileData = AES.AESDecrypt(encryptionData, AppConst.AbPackageKey);
                }
                else
                {
                    //不是加密的文件，尝试直接读取
                    fileData = base.ReadFile(filePath);
                }
            }
            else
            {
                FDebugger.Log("filePath为空");

                return null;
            }

            return fileData;
        }
        else
        {
            FDebugger.Log("读取Lua文件时，文件名为空");
        }

        return null;
    }
}
