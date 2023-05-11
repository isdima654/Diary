using Diary_dblayer;
using Diary_Models.Models;
using Diary_Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diary_Server.Conntrolers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private LocalAuthService _localAuthService = LocalAuthService.GetInstance();
        readonly EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
                                            Request.Headers["Token"]! : Guid.Empty.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        // авторизация
        public IActionResult AuthPost(string login, string password)
        {
            try
            {
                var token = _localAuthService.Auth(login, password);
                return Ok(new
                {
                    status = "ok",
                    token
                });
            }
            catch (Exception E)
            {
                return Unauthorized(new
                {
                    status = "fail",
                    message = E.Message
                });
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // получение пользователя по токену
        public IActionResult GetUserInfo()
        {
            try
            {
                return Ok(new
                {
                    status = "ok",
                    user = _localAuthService.GetUser(Token)
                });
            }
            catch (Exception E)
            {
                return Unauthorized(new
                {
                    status = "fail",
                    message = E.Message
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        //регистрация пользователя
        public IActionResult SignUp([FromBody] JObject json)
        {
            try
            {
                if (_db.GetUsers(x => x.Login == json["login"]?.ToString()).Any())
                    throw new Exception("User with this login exists");
                User potentialUser = new()
                {
                    Login = json["login"]?.ToString() ?? throw new Exception("Login is missing"),
                    Password = Extensions.ComputeSHA256(json["password"]?.ToString() ?? throw new Exception("Password is missing")),
                    Name = json["name"]?.ToString() ?? throw new Exception("Name is missing"),
                };
                _db.AddOrUpdate(potentialUser);
                return Ok(new
                {
                    status = "ok"
                });
            }
            catch (Exception E)
            {
                return BadRequest(new
                {
                    status = "fail",
                    message = E.Message
                });
            }
        }
    }
}
