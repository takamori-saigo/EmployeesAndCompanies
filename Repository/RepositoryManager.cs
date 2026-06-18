using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryManager: IRepositoryManager
{
    private readonly RepositoryContext _dbContext;
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;
    public ICompanyRepository Company => _companyRepository.Value;
    public IEmployeeRepository Employee => _employeeRepository.Value;

    public RepositoryManager(RepositoryContext dbContext)
    {
        _dbContext =  dbContext;
        _companyRepository =  new Lazy<ICompanyRepository>(() => new CompanyRepository(dbContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(dbContext));
    }
    
    public async Task SaveAsync()
    {
        _dbContext.SaveChangesAsync();
    }
}