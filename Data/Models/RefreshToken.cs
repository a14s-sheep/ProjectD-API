using System.ComponentModel.DataAnnotations;

namespace ProjectD_API.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public required string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpriesAt { get; set; }
        public DateTime? RevokedAt { get; set; }


    }
}
