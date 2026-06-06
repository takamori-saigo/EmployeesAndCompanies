namespace Contracts;

public interface IManagerRepository
{
    IEmployeeRepository Employee { get; }
    ICompanyRepository Company { get; }
    void Save();
}