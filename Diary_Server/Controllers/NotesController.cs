using Diary_dblayer;
using Diary_Models.Models;
using Diary_Server.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Diary_Server.Controllers
{
    /// <summary>
    /// 
    /// </summary>
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
        /// Note by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add or update note info
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
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
       /// <summary>
       /// sort for date
       /// </summary>
       /// <param name="start"></param>
       /// <param name="end"></param>
       /// <returns></returns>
        [HttpGet]
        [Route("filters/date/{start}-{end}")]
        public IActionResult GetDate([FromRoute] DateTime start,[FromRoute] DateTime end)
        {
            User user;
            if ((user = LocalAuthService.GetInstance().GetUser(Token)) is null)
                return Unauthorized(new
                {
                    status = "fail",
                    message = "Session is not valid"
                });
            var notes = user.Notes.Where(x => (x.Date > start) && (x.Date < end));
            return Ok(new
            {
                status = "ok",
                notes
            });
        }

        /// <summary>
        /// sort for status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("filters/status")]
        public IActionResult GetStatus()
        {
            User user;
            if ((user = LocalAuthService.GetInstance().GetUser(Token)) is null)
                return Unauthorized(new
                {
                    status = "fail",
                    message = "Session is not valid"
                });
            var marks = user.Notes.Where(x => x.Status != null);
            return Ok(new
            {
                status = "ok",
                marks
            });
        }
        /// <summary>
        /// sort for repeat
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("filters/repeat")]
        public IActionResult GetRepeat()
        {
            User user;
            if ((user = LocalAuthService.GetInstance().GetUser(Token)) is null)
                return Unauthorized(new
                {
                    status = "fail",
                    message = "Session is not valid"
                });
            var reset = user.Notes.Where(x => x.Repeat != null);
            return Ok(new
            {
                status = "ok",
                reset
            });
        }

    }
}