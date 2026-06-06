using Contracts;

namespace Repository;

public class ManagerRepository: IManagerRepository
{
    private readonly Lazy<EmployeeRepository> _employee;
    private readonly Lazy<CompanyRepository> _company;
    private readonly RepositoryContext _repositoryContext;
    public IEmployeeRepository Employee => _employee.Value;
    public ICompanyRepository Company => _company.Value;

    public ManagerRepository(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _employee = new Lazy<EmployeeRepository>(() => new EmployeeRepository(_repositoryContext));
        _company = new Lazy<CompanyRepository>(() => new CompanyRepository(_repositoryContext));
    }
    
    public void Save()
    {
        _repositoryContext.SaveChanges();
    }
}