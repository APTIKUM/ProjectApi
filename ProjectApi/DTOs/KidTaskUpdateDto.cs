using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProjectApi.DTOs
{
    public class KidTaskUpdateDto
    {
        [StringLength(255)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public int? Price { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool? IsCompleted { get; set; }
        public string? RepeatDaysJson { get; set; }
        public string? CompletedDatesJson { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}
