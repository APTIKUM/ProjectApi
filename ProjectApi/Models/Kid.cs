using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectApi.Models
{
    public class Kid
    {
        [Key]
        [StringLength(5)]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; }

        public int GameBalance { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        [JsonIgnore]
        public List<Parent> Parents { get; set; } = new();

        [JsonIgnore]
        public List<KidTask> Tasks { get; set; } = new();
    }

}
