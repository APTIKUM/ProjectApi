using System.ComponentModel.DataAnnotations;

namespace ProjectApi.DTOs;

public class ParentUpdateDto
{
    [StringLength(255, ErrorMessage = "Имя не должно превышать 255 символов")]
    public string? Name { get; set; }

    [StringLength(255, ErrorMessage = "URL аватарки не должен превышать 255 символов")]
    public string? AvatarUrl { get; set; }
}