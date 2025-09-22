using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectD_API.Data.Models
{
    public class ClassDefaultItem
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("CharacterClass")]
        public string ClassId { get; set; }
        public int DataId { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Amount { get; set; }
        public float Durability { get; set; }
    }
}
