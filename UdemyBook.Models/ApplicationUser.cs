using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UdemyBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
