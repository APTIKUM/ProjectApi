using System.ComponentModel.DataAnnotations;

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

        // Навигационное свойство для связи многие-ко-многим
        public ICollection<Parent> Parents { get; set; } = new List<Parent>();
    }

}
