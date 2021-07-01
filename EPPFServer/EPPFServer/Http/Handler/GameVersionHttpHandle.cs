using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using EPPFServer.Http.Attributes;

namespace EPPFServer.Http.Handler
{
    [HttpHandleAttribute("/GameVersion/")]
    public class GameVersionHttpHandle : HttpHandleBase
    {
        private GameVersionHttpHandle()
        {

        }

        /// <summary>
        /// 获取加载中的提示文字
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parametersDict"></param>
        [HttpHandleMethod("GetTip")]
        public void GetGameVersion(HttpListenerContext context, Dictionary<string, string> parametersDict)
        {
            Instance.HttpListenerResponse(context.Response, "{\"Version\": \"1.0.0\"}");
        }
    }
}
