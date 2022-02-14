using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Log
{
    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogEntity
    {
        /// <summary>
        /// 类型
        /// </summary>
        private string _logType = "";

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType
        {
            get { return _logType; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _logType = "";
                else
                    _logType = value + "\\";
            }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 是否输出
        /// </summary>
        public bool IsWrite { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 日常类型
    /// </summary>
    public enum LogTypeState
    {
        None = 0,//无
        Exception = 1,//异常
        Operation = 2,//操作
        Url = 3
    }
}
