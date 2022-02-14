using Tools.JWT;
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
        public ReturnModel Login([FromBody] UserModel user)
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

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        [HttpGet("ValiToken")]
        public ReturnModel ValiToken(string tokenStr)
        {
            var ret = new ReturnModel
            {
                TnToken = new TnToken()
            };
            bool validateBool = _tokenHelper.ValiToken(tokenStr);
            if (validateBool)
            {
                ret.Code = 200;
                ret.Msg = "Token验证成功";
                ret.TnToken.TokenStr = tokenStr;
            }
            else
            {
                ret.Code = 500;
                ret.Msg = "Token验证失败";
                ret.TnToken.TokenStr = tokenStr;
            }
            return ret;
        }

        /// <summary>
        /// 验证Token 带返回状态
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        [HttpGet("ValiTokenState")]
        public ReturnModel ValiTokenState(string tokenStr)
        {
            var ret = new ReturnModel
            {
                TnToken = new TnToken()
            };
            string loginId = "";
            TokenType tokenType = _tokenHelper.ValiTokenState(tokenStr
                , a => a["iss"] == "huangjie" && a["aud"] == "EveryOne"
                , action => { loginId = action["loginID"]; });
            if (tokenType == TokenType.Fail)
            {
                ret.Code = 202;
                ret.Msg = "token验证失败";
                return ret;
            }
            if (tokenType == TokenType.Expired)
            {
                ret.Code = 205;
                ret.Msg = "token已过期";
                return ret;
            }

            var data = new List<Dictionary<string, string>>();
            data.Add(new Dictionary<string, string>() {
                {"oh","123" }
            });
            ret.Code = 200;
            ret.Msg = "验证成功";
            ret.Data = data;
            return ret;
        }
    }
}
