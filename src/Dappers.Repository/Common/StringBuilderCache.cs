using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dappers.Repository
{
    public static class StringBuilderCache
    {
        [ThreadStatic]
        private static StringBuilder _cache;

        /// <summary>
        /// 获取并销毁
        /// </summary>
        /// <returns></returns>
        public static StringBuilder Allocate()
        {
            var stringBuilder = _cache;
            if (stringBuilder == null)
                return new StringBuilder();
            stringBuilder.Length = 0;
            _cache = null;
            return stringBuilder;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="stringBuilder"></param>
        public static void Free(StringBuilder stringBuilder)
        {
            _cache = stringBuilder;
        }

        /// <summary>
        /// 设置并返回
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        public static string ReturnAndFree(StringBuilder stringBuilder)
        {
            var str = stringBuilder.ToString();
            _cache = stringBuilder;
            return str;
        }
    }
}
