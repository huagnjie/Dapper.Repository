using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperTest.Busines;

namespace DapperTest.Controllers
{
    public static class BizInstance
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// 注册StudentBusines
        /// </summary>
        private static StudentBusines studentBusines = null;
        public static StudentBusines StudentBusines
        {
            get
            {
                if (studentBusines == null)
                {
                    lock (_lock)
                    {
                        if (studentBusines == null)
                            studentBusines = new StudentBusines();
                        return studentBusines;
                    }
                }
                return studentBusines;
            }
        }

    }
}
