using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Tools.Util;

namespace Tools.Log
{
    /// <summary>
    /// 阻塞队列Log
    /// </summary>
    public static class BlockQueueLog
    {
        private static BlockQueue<LogEntity> blockQueue = new BlockQueue<LogEntity>(10000000);
        private static bool _flag = false;
        private static Thread th = new Thread(ThreadWrite);

        /// <summary>
        /// 将日志数据写入队列
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strLogType"></param>
        /// <param name="strContent"></param>
        /// <param name="blIsWrite"></param>
        internal static void QueueWriteLog(string strPath, LogTypeState strLogType
            , string strContent, bool blIsWrite)
        {
            LogEntity entity = new LogEntity
            {
                Path = strPath,
                LogType = strLogType.ToString(),
                Content = strContent,
                IsWrite = blIsWrite
            };
            blockQueue.Enqueue(entity);
            if (!_flag && th.ThreadState != ThreadState.Running)
            {
                _flag = true;
                th.IsBackground = true;
                th.Start();
            }
        }

        /// <summary>
        /// 处理队列数据
        /// </summary>
        private static void ThreadWrite()
        {
            while (_flag)
            {
                if (blockQueue.Count() > 0
                    && blockQueue.TryDequeue(out LogEntity logEntity))
                {
                    QueueWriteLogFile(logEntity);
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 输出日志到文件
        /// </summary>
        /// <param name="logEntity"></param>
        private static void QueueWriteLogFile(LogEntity logEntity)
        {
            if (logEntity != null && logEntity.IsWrite)
            {
                StringBuilder strPath = new StringBuilder();
                strPath.Append(logEntity.Path + logEntity.LogType);
                if (!Directory.Exists(strPath.ToString()))
                {
                    Directory.CreateDirectory(strPath.ToString());
                }
                strPath.Append(DateTime.Now.ToString("yyyyMMdd") + ".log");
                var strFile = DirFileHelper.FileReNameBySize(strPath.ToString());
                using (StreamWriter sw = new StreamWriter(
                    strFile, true, Encoding.UTF8))
                {
                    sw.WriteLine(
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + " " + logEntity.Content);
                    sw.WriteLine("----------- cut-off line -----------");
                }
                DirFileHelper.DeleteFileByDay(30);
            }
        }

        public static void CloseThread()
        {
            th.Abort();
        }
    }
}
