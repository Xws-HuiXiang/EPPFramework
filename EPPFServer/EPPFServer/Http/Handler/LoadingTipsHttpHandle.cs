using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using EPPFServer.Common;
using EPPFServer.Http.Attributes;
using EPPFServer.Http.Data;
using EPPFServer.Log;
using EPPFServer.Network;

namespace EPPFServer.Http.Handler
{
    [HttpHandleAttribute("/LoadingTips/")]
    public class LoadingTipsHttpHandle : HttpHandleBase
    {
        /// <summary>
        /// 加载界面的提示文字数组
        /// </summary>
        private string[] loadingTipsArray;

        private LoadingTipsHttpHandle() : base()
        {
            LoadLoadingTips();
        }

        /// <summary>
        /// 加载 加载中提示文字 的内容
        /// </summary>
        private void LoadLoadingTips()
        {
            string loadingTipsFilePath = ServerSocket.Config.LoadingTipsFilePath.Trim();

            if (string.IsNullOrEmpty(loadingTipsFilePath))
            {
                Debug.L.Warn("获取加载提示文字文件的内容时，文件的路径为空");

                return;
            }

            loadingTipsArray = new string[]
            {
                "加载中提示1",
                "加载中提示2",
                "加载中提示3"
            };
        }

        /// <summary>
        /// 获取加载中的提示文字
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parametersDict"></param>
        [HttpHandleMethod("GetTip")]
        public void GetALoadingTip(HttpListenerContext context, Dictionary<string, string> parametersDict)
        {
            if(loadingTipsArray == null || loadingTipsArray.Length <= 0)
            {
                //没有提示的文字，返回请求错误的对象
                Instance.HttpListenerErrorResponse(context.Response, ResponseErrorCode.GetLoadingTipsError, "服务器中不存在提示文字内容");
            }
            else
            {
                Random r = new Random((int)ServerUtils.GetTimeStamp());
                int tipsIndex = r.Next(0, loadingTipsArray.Length);
                string content = loadingTipsArray[tipsIndex];
                GetALoadingTipData obj = new GetALoadingTipData() 
                {
                    Content = content
                };
                string jsonString = JsonMapper.ToJson(obj);

                Instance.HttpListenerResponse(context.Response, jsonString);
            }
        }
    }
}
