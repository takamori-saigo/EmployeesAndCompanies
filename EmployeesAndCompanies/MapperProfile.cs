using AutoMapper;
using Entities;
using Shared;
using Shared.DataTransferObjects;

namespace EmployeesAndCompanies;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<Company, CompanyDTO>()
            .ForMember(c => c.FullAddress, opt =>
                opt.MapFrom(x => $"{x.Address} {x.Country}"));
    
        CreateMap<Employee, EmployeeDTO>();
        
        CreateMap<CompanyForCreationDto, Company>();
        
        CreateMap<EmployeeForCreationDto, Employee>();
    }
}