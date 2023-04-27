using Diary_Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diary_Server.Conntrolers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private LocalAuthService _localAuthService = LocalAuthService.GetInstance();

        [HttpPost]
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
    }
}
