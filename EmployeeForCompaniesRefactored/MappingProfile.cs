using AutoMapper;
using Entities;
using Shared;

namespace EmployeeForCompaniesRefactored;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForCtorParam("FullAddress",
                opt =>
                    opt.MapFrom(x => string.Join(",", new string[]{x.Address, x.Country})));

        CreateMap<Employee, EmployeeDto>();
    }
}