using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Common.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel">令牌</param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt tokenModel)
        {
            string iss = AppSettingsHelper.App(new string[] { "Audience","Issuer"});
            string aud = AppSettingsHelper.App(new string[] { "Audience","Audience"});
            string secret = AppSettingsHelper.App(new string[] {"Audience","Secret" });

            var claims = new List<Claim>
            {
                /*
                 * 特别重要：
                 * 这里将用户的部分信息，比如Uid存到了Claim中，如果想知道如何在其他地方将这个uid从Token中取出来，则看SerializeJwt()方法，
                 * 也可以研究一下HttpContext.User.Calims，具体可以查看Policys/PermissionHandler.cs类中的使用
                 */

                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                //设置过期时间，默认设置过期为1200秒，可自定义，注意：Jwt有自己的缓冲过期时间
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1200)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration,DateTime.Now.AddSeconds(1200).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud)

            };

            //可将一个用户的多个角色全部赋予
            claims.AddRange(tokenModel.Role.Split(',').Select(s=>new Claim(ClaimTypes.Role,s)));

            //密钥（SymmetricSecurityKey对安全性的要求，密钥的长度太短会报出异常）
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer:iss,
                claims:claims,
                signingCredentials:creds
                );

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析Jwt
        /// </summary>
        /// <param name="jwtStr">jwt字符串</param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();

            //token校验
            if (!string.IsNullOrEmpty(jwtStr) && jwtHandler.CanReadToken(jwtStr))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

                object role;

                jwtToken.Payload.TryGetValue(ClaimTypes.Role,out role);

                tokenModelJwt = new TokenModelJwt
                {
                    Uid = Convert.ToInt64(jwtToken.Id),
                    Role = role == null ? "" : role.ToString()
                };
            }

            return tokenModelJwt;
        }

        /// <summary>
        /// 授权解析jwt
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static TokenModelJwt ParsingJwtToken(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                return null;
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            TokenModelJwt tm = SerializeJwt(tokenHeader);
            return tm;
        }

    }

    /// <summary>
    /// 令牌模型
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 职能
        /// </summary>
        public string Work { get; set; }
    }
}
