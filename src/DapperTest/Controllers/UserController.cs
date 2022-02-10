using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.Controllers
{
    public class UserController:BaseController
    {
        public UserController()
        {
              
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStudentList")]
        public ObjectResult GetStudentList()
        {
            return new ObjectResult(BizInstance.StudentBusines.GetStudentList());
        }
    }
}
