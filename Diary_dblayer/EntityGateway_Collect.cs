using Diary_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary_dblayer
{
    public partial class EntityGateway
    {
        public IEnumerable<User> GetUsers(Func<User, bool> predicate) =>
            Context.Users.Where(predicate).ToArray();
        public IEnumerable<User> GetUsers() =>
            GetUsers(x => true);

        public IEnumerable<Note> GetNotes(Func<Note, bool> predicate) =>
           Context.Notes.Where(predicate).ToArray();
        public IEnumerable<Note> GetNotes() =>
            GetNotes(x => true);
    }
}
