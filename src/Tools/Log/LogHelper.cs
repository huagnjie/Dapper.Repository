using System;
using Tools.Util;

namespace Tools.Log
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class LogHelper : ILogHelper
    {
        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="content">内容</param>
        public void WriteLog(string content)
        {
            WriteLog(LogTypeState.None, content);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="strings"></param>
        public void WriteLog(LogTypeState logType, string strings)
        {
            WriteLog(logType, strings, !string.IsNullOrEmpty(AppSettingsJson.Configuration["LogConfig:WriteLog"]));
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="strings"></param>
        /// <param name="isWrite">是否输出日志</param>
        public void WriteLog(LogTypeState logType, string strings, bool isWrite)
        {
            string strPath = BaseUtil.baseDirectory + @"\log\";
            WriteLog(strPath, logType, strings, isWrite);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="logType"></param>
        /// <param name="content"></param>
        /// <param name="isWrite"></param>
        public void WriteLog(string strPath, LogTypeState logType, string content, bool isWrite)
        {
            BlockQueueLog.QueueWriteLog(strPath, logType, content, isWrite);
        }

        /// <summary>
        /// 记录异常Log
        /// </summary>
        /// <param name="content"></param>
        public void WriteLogByException(string content)
        {
            WriteLog(LogTypeState.Exception, content, true);
        }

        /// <summary>
        /// 记录WebAPI Log
        /// </summary>
        /// <param name="content"></param>
        public void WriteLogByUrl(string content)
        {
            WriteFileUrl("", content);
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="userName"></param>
        public void WriteLogByOperation(string content, string userName = "")
        {
            content = "【消息】" + content;
            if (!string.IsNullOrEmpty(userName))
            {
                content = "【操作者】" + userName + content;
            }
            string strPath = BaseUtil.baseDirectory + @"\log\" + DateTime.Now.ToString("yyyyMM") + "\\";
            WriteLog(strPath, LogTypeState.Operation, content, true);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="category"></param>
        /// <param name="content"></param>
        public void WriteFileUrl(string category, string content)
        {
            WriteFileUrl(category, LogTypeState.Url, content);
        }

        public void WriteFileUrl(string category, LogTypeState logType, string content)
        {
            content = string.Format("【{0}】", category) + content;
            WriteLog(logType, content, true);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="logType">类型</param>
        /// <param name="Strings">内容</param>
        /// <param name="Write">是否输出日志</param>
        public void WriteFile(string strPath, LogTypeState logType, string Strings, string Write)
        {
            bool blWrite = false;
            if (Write != "" && ((bool.TryParse(Write, out blWrite) && blWrite) || Write.Equals("write", StringComparison.CurrentCultureIgnoreCase)))
            {
                blWrite = true;
            }
            WriteLog(strPath, logType, Strings, blWrite);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType">类型</param>
        /// <param name="Strings">内容</param>
        /// <param name="Write">是否输出日志</param>
        public void WriteFile(LogTypeState logType, string Strings, string Write)
        {
            string strPath = BaseUtil.baseDirectory + @"\log\";
            WriteFile(strPath, logType, Strings, Write);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType">类型</param>
        /// <param name="Strings">内容</param>
        public void WriteFile(LogTypeState logType, string Strings)
        {
            WriteFile(logType, Strings, AppSettingsJson.Configuration["LogConfig:WriteLog"]);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="Strings">内容</param>
        public void WriteFile(string Strings)
        {
            WriteFile(LogTypeState.None, Strings);
        }
    }
}
