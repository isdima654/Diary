using Diary_dblayer;
using Diary_Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diary_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private EntityGateway _db = new();

        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ?
                                         Request.Headers["Token"]! : Guid.Empty.ToString());
        /// <summary>
        /// All 
        /// </summary>

        public IActionResult GetAll() =>
            Ok(new
            {
                status = "ok",
                usersController = _db.GetUsers()
            });

        /// <summary>
        /// users title id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var potentialTitle = _db.GetUsers(x => x.Id == id).FirstOrDefault();
            if (potentialTitle is not null)
                return Ok(new
                {
                    status = "ok",
                    notestitles = potentialTitle
                });
            else
                return NotFound(new
                {
                    status = "fail",
                    message = $"There is no users title with {id} id"
                });
        }

        public IActionResult PostUsersTitle([FromBody] UserTitle userTitle)
        {
            if (LocalAuthService.GetInstance().GetUser(Token) == User.user)
                return Unauthorized(new
                {
                    status = "fail",
                    message = "You have no rights for that action."
                });
            _db.AddOrUpdate(userTitle);
            return Ok(new
            {
                status = "ok",
                id = userTitle.Id
            });
        }
    }
}
