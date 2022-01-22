using DapperTest.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.Controllers
{
    /// <summary>
    /// 返回类
    /// </summary>
    public class ReturnModel
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Token信息
        /// </summary>
        public TnToken TnToken { get; set; }
    }
}
