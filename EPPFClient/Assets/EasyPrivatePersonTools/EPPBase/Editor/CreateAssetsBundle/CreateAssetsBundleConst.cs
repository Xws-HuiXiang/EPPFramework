using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPPTools.PluginSettings
{
    /// <summary>
    /// 资源包打包范围
    /// </summary>
    public enum BuildBundleFunction
    {
        /// <summary>
        /// 所有标记的资源文件
        /// </summary>
        AllAssets = 0,
        /// <summary>
        /// 打包目标文件夹中的内容
        /// </summary>
        TargetFolder = 1,
        /// <summary>
        /// 分布式文件夹。根据目标文件夹下的所有文件夹，打包不同的bundle。bundle名称为文件所在的文件夹名称
        /// </summary>
        DistributedFolder = 2
    }
    public class CreateAssetsBundleConst
    {

    }
}
