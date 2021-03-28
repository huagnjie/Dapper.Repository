using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dappers.Repository
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        //数据库类型：SqlServer
        SqlServer = 0,
        //数据库类型：MySql
        MySql = 1,
        //数据库类型：Oracle
        Oracle = 2,
    }
}
