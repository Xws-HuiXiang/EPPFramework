using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPPFServer.Log
{
    public class Debug
    {
        private static ILoggerRepository repository;
        private static ILog log;
        public static ILog L { get { return log; } }

        static Debug()
        {
            //Log4net默认使用936编码（BG2312编码），这里指定为CodePagesEncodingProvider.Instance
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.ConfigureAndWatch(repository, new FileInfo("Log4net.config"));

            //初始化log字段
            log = LogManager.GetLogger(repository.Name, typeof(Debug));
        }
    }
}
