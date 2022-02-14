using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tools.JWT
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
        public TnToken CreateToken(Dictionary<string, string> user)
        {
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

        /// <summary>
        /// 验证身份 验证签名的有效性
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <param name="validatePayLoad"></param>
        /// <returns></returns>
        public bool ValiToken(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad = null)
        {
            var success = true;
            if (string.IsNullOrWhiteSpace(encodeJwt))
            {
                return false;
            }

            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3)
            {
                return false;
            }

            var header = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
            //取出来配置文件中的签名秘钥
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(_options.Value.IssuerSigningKey));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            success = string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
                return success;

            //验证是否在有效期中
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));
            if (!success)
                return success;

            //不存在自定义验则返回true
            if (validatePayLoad == null)
                return true;

            success = validatePayLoad(payLoad);
            return success;
        }

        /// <summary>
        /// 验证身份 验证签名的有效性
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <returns></returns>
        public TokenType ValiTokenState(string encodeJwt)
        {
            if (string.IsNullOrWhiteSpace(encodeJwt))
                return TokenType.Fail;
            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3)//数据格式都不对直接pass
                return TokenType.Fail;
            var header = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(_options.Value.IssuerSigningKey));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            if (!string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1]))))))
            {
                return TokenType.Fail;
            }
            //其次验证是否在有效期内（必须验证）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            if (!(now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString())))
            {
                return TokenType.Expired;
            } 
            return TokenType.Ok;
        }

        /// <summary>
        /// 验证身份 验证签名的有效性
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <param name="validatePayLoad"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public TokenType ValiTokenState(string encodeJwt, Func<Dictionary<string, string>, bool> validatePayLoad, Action<Dictionary<string, string>> action)
        {
            if (string.IsNullOrWhiteSpace(encodeJwt))
                return TokenType.Fail;
            var jwtArr = encodeJwt.Split('.');
            if (jwtArr.Length < 3)//数据格式都不对直接pass
                return TokenType.Fail;
            var header = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(_options.Value.IssuerSigningKey));
            //验证签名是否正确（把用户传递的签名部分取出来和服务器生成的签名匹配即可）
            if (!string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1]))))))
            {
                return TokenType.Fail;
            }
            //其次验证是否在有效期内（必须验证）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            if (!(now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString())))
            {
                return TokenType.Expired;
            }

            //不需要自定义验证不传或者传递null即可
            if (validatePayLoad == null)
            {
                action(payLoad);
                return TokenType.Ok;
            }
            //再其次 进行自定义的验证
            if (!validatePayLoad(payLoad))
            {
                return TokenType.Fail;
            }
            //可能需要获取jwt摘要里边的数据，封装一下方便使用
            action(payLoad);
            return TokenType.Ok;
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
    }
}
