using DapperTest.JWT;
using DapperTest.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ITokenHelper _tokenHelper = null;

        public LoginController(ITokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        [HttpPost("Login")]
        public ReturnModel Login([FromBody]UserModel user)
        {
            var ret = new ReturnModel();
            try
            {
                if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.PassWord))
                {
                    ret.Code = 201;
                    ret.Msg = "用户名密码不能为空";
                    return ret;
                }
                //登录操作 我就没写了 || 假设登录成功
                if (1 == 1)
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                    { 
                        { "UserName", user.UserName },
                        { "PassWord", user.PassWord }
                    };
                    ret.Code = 200;
                    ret.Msg = "登录成功";
                    ret.TnToken = _tokenHelper.CreateToken(keyValuePairs);
                }
            }
            catch (Exception ex)
            {
                ret.Code = 500;
                ret.Msg = "登录失败:" + ex.Message;
            }
            return ret;
        }
    }
}
