using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationcoredurgesh.Models
{
    public class Employee
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public int sal { get; set; }
        [Required]
        public string gmail { get; set; }
        [Required]
        public int password { get; set; } 
    }
}
