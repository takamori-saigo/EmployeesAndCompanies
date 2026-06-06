using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Employee
{
    [Column("EmployeeId")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "The field Name is required")]
    [MaxLength(30, ErrorMessage = "The field Name cannot exceed 30 characters")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "The field Age is required")]
    public int Age { get; set; }
    [Required(ErrorMessage = "The field Position is required")]
    [MaxLength(20, ErrorMessage = "The field Position cannot exceed 20 characters")]
    public string? Position { get; set; }
    [ForeignKey(nameof(Company))]
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}