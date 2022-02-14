using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.JWT
{
    /// <summary>
    /// 存放Token和过期时间
    /// </summary>
    public class TnToken
    {
        /// <summary>
        /// Token
        /// </summary>
        public string TokenStr { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
