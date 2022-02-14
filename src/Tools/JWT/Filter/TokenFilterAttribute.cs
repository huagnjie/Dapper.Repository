using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.JWT.Filter
{
    /// <summary>
    /// 过滤器，判断是否需要Token验证
    /// </summary>
    public class TokenFilterAttribute : Attribute, IActionFilter
    {
        private ITokenHelper tokenHelper;
        public TokenFilterAttribute(ITokenHelper _tokenHelper)
        {
            tokenHelper = _tokenHelper;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ReturnModel ret = new ReturnModel();
            //获取Token
            string token = null;//地址栏
            if (context.HttpContext.Request.Query.ContainsKey("Token"))
            {
                token = context.HttpContext.Request.Query["Token"].ToString();
            }
            else
            {                
                //header获取
                if (context.HttpContext.Request.Headers.ContainsKey("Token"))
                {
                    token = context.HttpContext.Request.Headers["Token"].ToString();
                }
                else if(context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", " ").TrimStart(' ');
                }
            }
            if (token == null)
            {
                ret.Code = 201;
                ret.Msg = "请登录";
                context.Result = new JsonResult(ret);
                return;
            }
            //验证jwt
            TokenType tokenType = tokenHelper.ValiTokenState(token);
            if (tokenType == TokenType.Fail)
            {
                ret.Code = 202;
                ret.Msg = "token验证失败";
                context.Result = new JsonResult(ret);
                return;
            }
            if (tokenType == TokenType.Expired)
            {
                ret.Code = 205;
                ret.Msg = "token已经过期";
                context.Result = new JsonResult(ret);
            }
            //给控制器传递参数(需要什么参数其实可以做成可以配置的，在过滤器里边加字段即可)
        }
    }
}
