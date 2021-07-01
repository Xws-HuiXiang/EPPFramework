using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using EPPFServer.Http.Data;
using EPPFServer.Log;

namespace EPPFServer.Http.Handler
{
    /// <summary>
    /// Http请求处理基类
    /// </summary>
    public class HttpHandleBase
    {
        /// <summary>
        /// 每个处理类都是一个单例类。这个值是在初始化时候，反射赋值的
        /// </summary>
        private HttpHandleBase instance;
        public HttpHandleBase Instance { get { return instance; } }

        /// <summary>
        /// 表示UTF-8编码的字符串
        /// </summary>
        public const string UTF_8_STRING = "UTF-8";

        public enum HttpContentType
        {
            /// <summary>
            /// html格式
            /// </summary>
            TextHtml,
            /// <summary>
            /// 纯文本格式
            /// </summary>
            TextPlain,
            /// <summary>
            /// xml格式
            /// </summary>
            TextXml,
            /// <summary>
            /// gif图片格式
            /// </summary>
            ImageGif,
            /// <summary>
            /// jpg图片格式
            /// </summary>
            ImageJpeg,
            /// <summary>
            /// png图片格式
            /// </summary>
            ImagePng,
            /// <summary>
            /// XHTML格式
            /// </summary>
            ApplicationXHtml,
            /// <summary>
            /// XML数据格式
            /// </summary>
            ApplicationXml,
            /// <summary>
            /// Atom XML聚合格式
            /// </summary>
            ApplicationAtomAndXml,
            /// <summary>
            /// Json数据格式
            /// </summary>
            ApplicationJson,
            /// <summary>
            /// PDF文件格式
            /// </summary>
            ApplicationPdf,
            /// <summary>
            /// 微软的Word文档格式
            /// </summary>
            ApplicationMSWord,
            /// <summary>
            /// 二进制流数据
            /// </summary>
            ApplicationOctetStream,
            /// <summary>
            /// 表单中默认的encType，form表单数据被编码为key/value格式发送到服务器（表单默认的提交数据的格式）
            /// </summary>
            ApplicationXWWWFormUrlencoded,
            /// <summary>
            /// 需要在表单中进行文件上传时需要使用该格式
            /// </summary>
            ApplicationFormData
        }

        /// <summary>
        /// 响应出现异常时的错误码
        /// </summary>
        public enum ResponseErrorCode
        {
            /// <summary>
            /// 获取加载中提示文字失败，服务器中没有提示文字
            /// </summary>
            GetLoadingTipsError = 4000
        }
        protected HttpHandleBase()
        {
            instance = this;
        }

        /// <summary>
        /// 从一个http响应中读取响应内容的字符串
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual string GetResponseString(WebResponse response)
        {
            var responseResult = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
            {
                responseResult = reader.ReadToEnd();
            }

            return responseResult;
        }

        /// <summary>
        /// 返回http的响应
        /// </summary>
        /// <param name="response"></param>
        /// <param name="content">这里内容应为json格式</param>
        /// <param name="statusCode"></param>
        public virtual void HttpListenerResponse(HttpListenerResponse response, string content, int statusCode = 200/*, HttpContentType contentType = HttpContentType.ApplicationJson*/)
        {
            string contentTypeString = GetContentTypeByEnum(HttpContentType.ApplicationJson);//默认全部为json格式

            response.StatusCode = statusCode;
            response.ContentType = contentTypeString;
            response.ContentEncoding = Encoding.UTF8;
            response.AppendHeader("Content-Type", contentTypeString);

            using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
            {
                writer.Write(content);
                writer.Close();
                response.Close();
            }
        }

        /// <summary>
        /// 返回请求失败的消息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="errorCode"></param>
        /// <param name="content"></param>
        /// <param name="statusCode"></param>
        public virtual void HttpListenerErrorResponse(HttpListenerResponse response, ResponseErrorCode errorCode, string content, int statusCode = 400/*, HttpContentType contentType = HttpContentType.ApplicationJson*/)
        {
            string contentTypeString = GetContentTypeByEnum(HttpContentType.ApplicationJson);//全部为json格式

            response.StatusCode = statusCode;
            response.ContentType = contentTypeString;
            response.ContentEncoding = Encoding.UTF8;
            response.AppendHeader("Content-Type", contentTypeString);

            //构建数据对象转换为json串
            ErrorResponseData obj = new ErrorResponseData();
            obj.ErrorCode = (int)errorCode;
            obj.Message = content;
            string jsonContent = JsonMapper.ToJson(obj);

            using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
            {
                writer.Write(jsonContent);
                writer.Close();
                response.Close();
            }
        }

        /// <summary>
        /// 根据枚举类型获取ContentType的字符串
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetContentTypeByEnum(HttpContentType contentType)
        {
            string res;
            switch (contentType)
            {
                case HttpContentType.TextHtml:
                    res = "text/html;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.TextPlain:
                    res = "text/plain;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.TextXml:
                    res = "text/xml;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ImageGif:
                    res = "image/gif;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ImageJpeg:
                    res = "image/jpeg;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ImagePng:
                    res = "image/png;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationXHtml:
                    res = "application/xhtml+xml;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationXml:
                    res = "application/xml;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationAtomAndXml:
                    res = "application/atom+xml;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationJson:
                    res = "application/json;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationPdf:
                    res = "application/pdf;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationMSWord:
                    res = "application/msword;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationOctetStream:
                    res = "application/octet-stream;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationXWWWFormUrlencoded:
                    res = "application/x-www-form-urlencoded;charset=" + UTF_8_STRING;
                    break;
                case HttpContentType.ApplicationFormData:
                    res = "multipart/form-data;charset=" + UTF_8_STRING;
                    break;
                default:
                    Debug.L.Warn(string.Format("遇到未知的内容类型：{0}。将默认为纯文本格式", nameof(contentType)));
                    res = "text/plain;charset=" + UTF_8_STRING;

                    break;
            }

            return res;
        }
    }
}
