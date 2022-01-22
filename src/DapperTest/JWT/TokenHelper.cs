using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DapperTest.JWT
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IOptions<JWTConfig> _options;
        public TokenHelper(IOptions<JWTConfig> options)
        {
            _options = options;
        }

        /// <summary>
        /// 根据一个对象通过反射提供负载生成Token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <returns></returns>
        public TnToken CreateToken<T>(T user) where T : class
        {
            //携带的负载部分，类似于一个键值对
            List<Claim> claims = new List<Claim>();
            //使用反射获取model数据
            foreach (var item in user.GetType().GetProperties())
            {
                object obj = item.GetValue(user);
                string value = "";
                if (obj != null)
                    value = obj.ToString();
                claims.Add(new Claim(item.Name, value));
            }
            //创建Token
            return CreateTokenString(claims);
        }


        /// <summary>
        /// 根据一个键值对对象通过反射提供负载生成Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public TnToken CreateToken(Dictionary<string, string> user) {
            //携带的负载部分，类似于一个键值对
            List<Claim> claims = new List<Claim>();
            //使用反射获取model数据
            foreach (var item in user)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }
            //创建Token
            return CreateTokenString(claims);
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims">List的 Claim对象</param>
        /// <returns></returns>
        private TnToken CreateTokenString(List<Claim> claims)
        {
            var now = DateTime.Now;
            var expires = now.Add(TimeSpan.FromMinutes(_options.Value.AccessTokenExpiresMinutes));
            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,//Token发布者
                audience: _options.Value.Audience,//Token接受者
                claims: claims,//携带的负载
                notBefore: now,//Token生成时间
                expires: expires,//过期时间
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey))
                    , SecurityAlgorithms.HmacSha256)
                );
            return new TnToken
            {
                TokenStr = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expires
            };
        }
    }
}
