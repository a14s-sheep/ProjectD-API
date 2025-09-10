using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class User
    {
        [Key]
        public required string Id { get; set; }
        [Required, MaxLength(36)]
        public required string Username { get; set; }
        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
