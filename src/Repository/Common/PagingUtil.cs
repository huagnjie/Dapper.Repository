using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository
{
    public class PagingUtil
    {
        private static readonly Regex _rexSelect = new Regex(@"^\s*SELECT\s+(.+?)\sFROM\s", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex _rexSelect1 = new Regex(@"^\s*SELECT\s+(.+?)\sFROM\s*\(+\s*", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex _rexOrderBy = new Regex(@"\s+ORDER\s+BY\s+([^\s]+(?:\s+ASC|\s+DESC)?)\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 分割SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PartedSql SplitSql(string sql)
        {
            var parts = new PartedSql { Raw = sql };

            //Extract the sql from "SELECT <whatever> FROM"
            var _s = _rexSelect1.Match(sql);
            if (_s.Success)
            {
                parts.Select = _s.Groups[1].Value;
                sql = sql.Substring(_s.Length);
                _s = _rexOrderBy.Match(sql);
                if (_s.Success)
                {
                    sql = sql.Substring(0, _s.Index);
                    parts.OrderBy = _s.Groups[1].Value;
                }
                parts.Body = "(" + sql;
                return parts;
            }

            var _m = _rexSelect.Match(sql);
            if (!_m.Success)
                throw new ArgumentException("Unable to parse SQL statement for select");

            parts.Select = _m.Groups[1].Value;
            sql = sql.Substring(_m.Length);
            if (_m.Success)
            {
                sql = sql.Substring(0, _m.Length);
                parts.OrderBy = _m.Groups[1].Value;
            }
            parts.Body = sql;
            return parts;
        }

        public static string GetCountSql(PartedSql sql)
        {
            return $"SELECT COUNT(1) FROM {sql.Body}";
        }
    }
}
