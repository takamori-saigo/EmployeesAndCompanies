using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "cfb56c1a-ef2a-4f47-9919-5f3fedd9efe5",
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole
            {
                Id = "a2282c1d-bd26-472f-82b5-ccc78784a027",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
    }
}

