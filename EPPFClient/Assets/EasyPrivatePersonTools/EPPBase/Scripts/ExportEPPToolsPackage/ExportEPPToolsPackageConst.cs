using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPPTools.ExportHandler
{
    /// <summary>
    /// 生成unitypackage资源包时名称后缀
    /// </summary>
    public enum PackageTestFlag
    {
        /// <summary>
        /// 稳定版。没有后缀
        /// </summary>
        NoneFlag,
        /// <summary>
        /// bate测试版
        /// </summary>
        B,
        /// <summary>
        /// alpha先行版
        /// </summary>
        A,
        /// <summary>
        /// resource稳定版
        /// </summary>
        R
    }
    public class ExportEPPToolsPackageConst
    {
        public const string NO_README_FILE = "No 'Readme.md' File";
        public const string NO_VERSION = "No Version";
    }
}
