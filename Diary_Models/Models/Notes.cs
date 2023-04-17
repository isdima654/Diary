using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Diary_Models.Models
{
    public class Notes
    {
        public Guid Id { get; set; }
        [Required] [StringLength(50)] public string Name { get; set; }
        [Required] [StringLength(50)] public string Place { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        [Required] [StringLength(500)] public string Description { get; set; }
        public string Status { get; set; }
        public string Repeat { get; set; }
        
        public virtual User User { get; set; }
    }
}
