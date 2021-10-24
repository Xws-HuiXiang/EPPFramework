using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using EPPFramework.Common;
using EPPFramework.Http.Attributes;
using EPPFramework.Http.Data;
using EPPFramework.Log;
using EPPFramework.Network;

namespace EPPFramework.Http.Handler
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
        private async void LoadLoadingTips()
        {
            string loadingTipsFilePath = ServerSocket.Config.LoadingTipsFilePath.Trim();

            if (string.IsNullOrEmpty(loadingTipsFilePath))
            {
                Debug.L.Warn("获取加载提示文字文件的内容时，文件的路径为空");

                return;
            }

            FilePathRouting routing = ServerUtils.GetLoadingTipsRouting(loadingTipsFilePath);
            switch (routing)
            {
                case FilePathRouting.Default:
                    //Default默认为local类型
                case FilePathRouting.Local:
                    string fileFullPath = Path.Combine(ServerUtils.ServerExeFolderPath, loadingTipsFilePath.Substring(8));
                    if (File.Exists(fileFullPath))
                    {
                        loadingTipsArray = await File.ReadAllLinesAsync(fileFullPath);
                    }
                    else
                    {
                        Debug.L.Warn(string.Format("不存在LoadingTips的文件：{0}", fileFullPath));
                    }
                    break;
                case FilePathRouting.Http:
                case FilePathRouting.Https:
                    try
                    {
                        WebRequest www = WebRequest.Create(loadingTipsFilePath);
                        WebResponse response = await www.GetResponseAsync();
                        string tipsFileString = GetResponseString(response);
                        tipsFileString = tipsFileString.Replace("\r", "");
                        loadingTipsArray = tipsFileString.Split('\n');
                    }
                    catch (Exception e)
                    {
                        Debug.L.Error(string.Format("加载Tips文件失败：{0}", e.Message));
                    }
                    break;
                case FilePathRouting.File:
                    loadingTipsArray = await File.ReadAllLinesAsync(loadingTipsFilePath.Substring(7));
                    break;
                default:
                    Debug.L.Warn(string.Format("遇到未知的路由类型：{0}", nameof(routing)));

                    return;
            }
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
