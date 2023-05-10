using Diary_dblayer;
using Diary_Models.Models;
using Diary_Server.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Diary_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
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
                notesController = _db.GetNotes()
            });

        /// <summary>
        /// notes title id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var potentialTitle = _db.GetNotes(x => x.Id == id).FirstOrDefault();
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
                    message = $"There is no notes title with {id} id"
                });
        }

        [HttpPost]
        public IActionResult PostNote([FromBody] Note note)
        {
            try
            {
                User user;
                if ((user = LocalAuthService.GetInstance().GetUser(Token)) is null)
                    return Unauthorized(new
                    {
                        status = "fail",
                        message = "Session is not valid"
                    });
                note.User = user;
                _db.AddOrUpdate(note);
                return Ok(new
                {
                    status = "ok",
                    id = note.Id
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