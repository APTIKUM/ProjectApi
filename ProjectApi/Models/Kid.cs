using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectApi.Models
{
    public class Kid
    {
        [Key]
        [StringLength(5)]
        public string Id { get; set; } = string.Empty;

        [StringLength(255)]
        public string Name { get; set; } = "Ребёнок";

        public int GameBalance { get; set; } = 0;

        [StringLength(255)]
        public string? AvatarUrl { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Parent> Parents { get; set; } = [];

        [JsonIgnore]
        public List<KidTask> Tasks { get; set; } = [];
    }

}
