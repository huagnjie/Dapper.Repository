using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Log;

namespace DapperTest.Controllers
{
    public class UserController:BaseController
    {
        private readonly ILogHelper _logHelper;
        public UserController(ILogHelper logHelper)
        {
            this._logHelper = logHelper;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStudentList")]
        public ObjectResult GetStudentList()
        {
            _logHelper.WriteLog("黄杰");
            return new ObjectResult(BizInstance.StudentBusines.GetStudentList());
        }         
    }
}
