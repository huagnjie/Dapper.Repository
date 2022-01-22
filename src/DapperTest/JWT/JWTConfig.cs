using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.JWT
{
    /// <summary>
    /// 配置Token生成信息
    /// </summary>
    public class JWTConfig
    {
        /// <summary>
        /// Token发布者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token接受者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string IssuerSigningKey { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int AccessTokenExpiresMinutes { get; set; }
    }
}
