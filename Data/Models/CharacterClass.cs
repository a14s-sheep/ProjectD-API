using System.ComponentModel.DataAnnotations;

namespace ProjectD_API.Data.Models
{
    public class CharacterClass
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int DefaultStatId { get; set; }
    }
}
