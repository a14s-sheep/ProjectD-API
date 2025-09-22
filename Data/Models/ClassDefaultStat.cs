using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class ClassDefaultStat
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("CharacterClass")]
        public string ClassId { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
    }
}
