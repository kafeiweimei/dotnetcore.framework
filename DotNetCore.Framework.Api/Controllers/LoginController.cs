using Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Framework.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //获取令牌
        [HttpGet]
        public async Task<object> GetJwtStr(string name, string password)
        {
            //将用户Id和角色名称作为单独的自定义变量，封装进token字符串中
            TokenModelJwt tokenModel = new TokenModelJwt { Uid=1,Role="Admin"};
            var jwtStr = JwtHelper.IssueJwt(tokenModel);//登陆，获取到一定规则的Token令牌
            var suc = true;
            return Ok(new {success=suc,token=jwtStr});
        }

    }
}
