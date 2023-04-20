using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Diary_Models.Models
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        [Required] [StringLength(50)]public string Name { get; set; }
        [Required] [StringLength(20)] public string Password { get; set; }
    }
}
