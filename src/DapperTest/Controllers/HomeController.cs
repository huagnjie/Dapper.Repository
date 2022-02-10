using DapperTest.JWT.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.Controllers
{
    //[Produces("application/json")]
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }
        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStudentList")]
        public ObjectResult GetStudentList() {
            return new ObjectResult(BizInstance.StudentBusines.GetStudentList());
        }
    }
}
