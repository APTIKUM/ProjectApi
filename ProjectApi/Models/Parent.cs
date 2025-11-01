using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Models
{
    public class Parent
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string? AvatarUrl { get; set; }
    }
}
