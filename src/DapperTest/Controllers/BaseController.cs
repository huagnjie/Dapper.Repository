using DapperTest.JWT.Filter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.Controllers
{
    [ServiceFilter(typeof(TokenFilter))]
    [Route("api/[Controller]")]
    [ApiController]

    public class BaseController:Controller
    {
    }
}
