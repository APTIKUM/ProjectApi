using System.ComponentModel.DataAnnotations;

namespace ProjectApi.DTOs
{
    public class ParentRegisterDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
