using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Log
{
    public interface ILogHelper
    {
        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="content">内容</param>
        void WriteLog(string content);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="strings"></param>
        void WriteLog(LogTypeState logType, string strings);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="strings"></param>
        /// <param name="isWrite">是否输出日志</param>
        void WriteLog(LogTypeState logType, string strings, bool isWrite);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="logType"></param>
        /// <param name="content"></param>
        /// <param name="isWrite"></param>
        void WriteLog(string strPath, LogTypeState logType, string content, bool isWrite);

        /// <summary>
        /// 记录异常Log
        /// </summary>
        /// <param name="content"></param>
        void WriteLogByException(string content);

        /// <summary>
        /// 记录WebAPI Log
        /// </summary>
        /// <param name="content"></param>
        void WriteLogByUrl(string content);

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="content"></param>
        /// <param name="userName"></param>
        void WriteLogByOperation(string content, string userName = "");

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="category"></param>
        /// <param name="content"></param>
        void WriteFileUrl(string category, string content);

        void WriteFileUrl(string category, LogTypeState logType, string content);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="logType">类型</param>
        /// <param name="Strings">内容</param>
        /// <param name="Write">是否输出日志</param>
        void WriteFile(string strPath, LogTypeState logType, string Strings, string Write);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType">类型</param>
        /// <param name="Strings">内容</param>
        /// <param name="Write">是否输出日志</param>
        void WriteFile(LogTypeState logType, string Strings, string Write);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="logType">类型</param>
        /// <param name="Strings">内容</param>
        void WriteFile(LogTypeState logType, string Strings);

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="Strings">内容</param>
        void WriteFile(string Strings);
    }
}
