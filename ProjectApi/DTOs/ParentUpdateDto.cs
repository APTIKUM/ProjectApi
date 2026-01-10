using System.ComponentModel.DataAnnotations;

namespace ProjectApi.DTOs;

public class ParentUpdateDto
{
    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [StringLength(255)]
    public string? DeviceToken { get; set; }
}