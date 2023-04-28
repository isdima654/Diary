using Diary_dblayer;
using Diary_Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Diary_Server.Services
{
    internal class LocalAuthService
    {
        class Session
        {
            public User User { get; set; }
            public DateTime LastOp { get; set; }
            public Guid Token { get; set; }
            public bool IsActive => DateTime.Now - LastOp < TimeSpan.FromHours(1);
        }

        public void CleanSessions() =>
            Task.Run(() =>
                Sessions.RemoveWhere(x => !x.IsActive));

        private static LocalAuthService? _instance;
        public static LocalAuthService GetInstance() => _instance ??= new();
        private LocalAuthService() { }
        private HashSet<Session> Sessions { get; set; } = new();

        private readonly EntityGateway _db = new();

        public Guid Auth(string login, string password)
        {
            var passhash = Extentions.ComputeSHA256(password);
            var potentialUser = _db.GetUsers(x => x.Login == login && x.Password == passhash).FirstOrDefault() ?? throw new Exception("User is not found");

            var Token = Guid.NewGuid();
            Sessions.Add(new()
            {
                LastOp = DateTime.Now,
                Token = Token,
                User = potentialUser
            });
            return Token;
        }

        public User GetUser(Guid token)
        {
            CleanSessions();
            return Sessions.FirstOrDefault(x => x.Token == token)?.User ?? throw new Exception("Session is not found");
        }
    }
}
