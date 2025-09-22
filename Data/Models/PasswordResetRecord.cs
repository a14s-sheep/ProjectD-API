using System.ComponentModel.DataAnnotations;

namespace ProjectD_API.Data.Models
{
    public class PasswordResetRecord
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int PIN { get; set; }
        [Required]
        public DateTime ExpiryTime { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
