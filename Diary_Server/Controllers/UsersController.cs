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
        /// Users by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Employees on the project
        /// </summary>
        [HttpGet]
        [Route("{id}/employees")]
        public IActionResult GetDate(DateTime date)
        {

        }    
    }
}
