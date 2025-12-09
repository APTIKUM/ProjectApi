using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

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

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }

        public bool IsCompleted { get; set; } = false;
        public bool IsRepetitive { get; set; } = false;
        public string RepeatDaysJson { get; set; } = "[]";
        public string CompletedDatesJson { get; set; } = "[]";

        [JsonIgnore]
        public Kid? Kid { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<DayOfWeek> RepeatDays
        {
            get => JsonSerializer.Deserialize<List<DayOfWeek>>(RepeatDaysJson ?? "[]") ?? [];
            set => RepeatDaysJson = JsonSerializer.Serialize(value);
        }

        [JsonIgnore]
        [NotMapped]
        public List<DateOnly> CompletedDates
        {
            get => JsonSerializer.Deserialize<List<DateOnly>>(CompletedDatesJson) ?? [];
            set => CompletedDatesJson = JsonSerializer.Serialize(value);
        }
    }
}
