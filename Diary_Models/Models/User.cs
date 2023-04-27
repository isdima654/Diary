using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Diary_Models.Models
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        [Required] [StringLength(100)]public string Name { get; set; }
        [Required] [StringLength(100)] public string Login { get; set; }
        [Required] [StringLength(100)] public string Password { get; set; }
    }
}
