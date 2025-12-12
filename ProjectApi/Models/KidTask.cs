using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProjectApi.Models
{
    public class KidTask
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string KidId { get; set; } = string.Empty;

        [StringLength(255)]
        public string Title { get; set; } = "Новая задача";

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int Price { get; set; } = 0;

        [Required]
        public DateTime TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public bool IsRepetitive => RepeatDays.Count > 0;
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
