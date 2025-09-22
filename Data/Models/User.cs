using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectD_API.Data.Models
{
    public class User : IdentityUser
    {
        [Key]
        public required string Id { get; set; }
        //[Required]
        //public string Email { get; set; }
        //[Required]
        //public string UserName { get; set; }
        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
