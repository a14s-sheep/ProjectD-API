using System.ComponentModel.DataAnnotations;

namespace ProjectD_API.Data.Models
{
    public class GameServerInfo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
    }
}
