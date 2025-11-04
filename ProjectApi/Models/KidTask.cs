using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectApi.Models
{
    public class KidTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string KidId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        
        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }

        public bool IsCompleted { get; set; } = false;
        public bool IsRepetitive { get; set; } = false;
        public string RepeatDaysJson { get; set; } = "[]";

        [JsonIgnore]
        public Kid? Kid { get; set; }

        [NotMapped]
        public List<DayOfWeek> RepeatDays
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<DayOfWeek>>(RepeatDaysJson ?? "[]") ?? new();
            set => RepeatDaysJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}
