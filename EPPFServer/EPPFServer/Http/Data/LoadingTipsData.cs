using System;
using System.Collections.Generic;
using System.Text;

namespace EPPFServer.Http.Data
{

    /// <summary>
    /// 获取加载中文字提示的数据对象
    /// </summary>
    public class GetALoadingTipData
    {
        /// <summary>
        /// 提示文字具体内容
        /// </summary>
        public string Content { get; set; }
    }
}
