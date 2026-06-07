using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Configurations;

namespace Repository;

public class RepositoryContext: DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeConfigurations());
        modelBuilder.ApplyConfiguration(new CompanyConfigurations());
    }

    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}