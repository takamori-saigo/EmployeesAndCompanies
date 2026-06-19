namespace Shared.RequestParameters;

public class EmployeeParameters: RequestParameters 
{
    public EmployeeParameters() => OrderBy = "name";
    public uint minAge { get; set; }
    public uint maxAge { get; set; } = int.MaxValue;
    public bool ValidAgeRange => maxAge > minAge;
    public string? SearchTerm { get; set; }
}