using Diary_Models;
using Diary_Models.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Diary_dblayer
{
    internal class DiaryContext: DbContext
    {
        private const string settingsPath = "db_settings.json";

        public DbSet <User> Users { get; set; }
        public DbSet <Note> Notes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(settingsPath))
                throw new FileNotFoundException("Config file is not found!");
            var json = File.ReadAllText(settingsPath);
            var jObj = JObject.Parse(json);
            var connectionString = jObj["connectionString"]?.ToString() ??
                throw new KeyNotFoundException("connectionString if missing.");

            optionsBuilder
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in typeof(IEntity)
                                    .Assembly
                                    .GetTypes()
                                    .Where(x =>typeof(IEntity).IsAssignableFrom(x) && x.IsClass))
                    modelBuilder.Entity(type)
                        .Property(nameof(IEntity.Id))
                        .HasDefaultValueSql("NEWSEQUENTIALID()");
                
        }
    }
}
