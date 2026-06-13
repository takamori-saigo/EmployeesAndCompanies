using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class EmployeeForUpdatingDto
(
    [Required(ErrorMessage = "The Name is required")]
    [MaxLength(30, ErrorMessage = "The Name cannot exceed 30 characters")]
    string? Name,
    [Required(ErrorMessage = "The Age is required")]
    int Age,
    [Required(ErrorMessage = "The Position is required")]
    [MaxLength(20, ErrorMessage = "The Position cannot exceed 20 characters")]
    string? Position
);