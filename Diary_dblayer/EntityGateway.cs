using Diary_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary_dblayer
{
    public partial class EntityGateway
    {
        internal DiaryContext Context { get; set; } = new DiaryContext();

        public void AddOrUpdate (params IEntity[] entities)
        {
            var toAdd = entities.Where(x => x.Id == Guid.Empty);
            var toUpdate = entities.Except(toAdd);
            Context.AddRange(toAdd);
            Context.UpdateRange(toUpdate);
            Context.SaveChanges();
        }

        public void Delete(params IEntity[] entities)
        {
            Context.RemoveRange(entities);
            Context.SaveChanges();
        }
        /// <summary>
        /// Employees on the project
        /// </summary>
        /* [HttpGet]
         [Route("{id}/employees")]
         public IActionResult GetEmployeesInProject([FromRoute] Guid id)
         {
             var potentialProject = _db.GetProjects(x => x.Id == id).FirstOrDefault();
             return potentialProject is null ?
                    NotFound(new
                    {
                        status = "fail",
                        message = $"There is no project with this id {id}!"
                    }) :
                    Ok(new
                    {
                        status = "ok",
                        employees = potentialProject.Employees
                    });
         }

         /// <summary>
         /// change employees from project
         /// </summary>
         /// <param name="action"></param>
         /// <param name="id">project id</param>
         /// <param name="employees">Json array of employees id</param>
         /// <returns></returns>
         [HttpPost]
         [Route("{id}/employees/{action}")]
         public IActionResult ManipulateEmployeesInProject([FromRoute] ActionType action, [FromRoute] Guid id, [FromBody] Guid[] employees)
         {
             try
             {
                 if (LocalAuthService.GetInstance().GetRole(Token) != Role.Admin)
                     return Unauthorized(new
                     {
                         status = "fail",
                         message = "You have no rights for this op."
                     });

                 _db.EmployeesInProject(action, id, employees);
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
         }*/
    }
}
