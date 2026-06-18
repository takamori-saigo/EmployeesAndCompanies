using System.ComponentModel.DataAnnotations;

namespace Shared;

public record CompanyForCreatiionDto
{
    [Required(ErrorMessage = "Employee name is required")]
    [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 characters")]
    public string? Name { get; init; }
    [Required(ErrorMessage = "Address name is required")]
    [MaxLength(20, ErrorMessage = "Maximum length for the address is 20 characters")]
    public string? Address { get; init; }
    [Required(ErrorMessage = "Country name is required")]
    [MaxLength(20, ErrorMessage = "Maximum length for the country is 20 characters")]
    public string? Country { get; init; }
    public IEnumerable<EmployeeForCreationDto> Employees { get; init; }
};