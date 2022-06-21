using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationcoredurgesh.Models
{
    public class User_Info
    {
        [Key]
        public string name { get; set; }

        [Required]
        public string gmail { get; set; }
        [Required]
        public string password { get; set; }
    }
}
