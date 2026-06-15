using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Configurations;

namespace Repository;

public class RepositoryContext: DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) 
        : base(options){}
    
    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
}