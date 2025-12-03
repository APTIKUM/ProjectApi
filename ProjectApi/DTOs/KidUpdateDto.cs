using System.ComponentModel.DataAnnotations;

namespace ProjectApi.DTOs
{
    public class KidUpdateDto
    {
        [StringLength(255)]
        public string? Name { get; set; }

        public int? GameBalance { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }
    }
}
