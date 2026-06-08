using AutoMapper;
using Entities;
using Shared.DataTransferObjects;

namespace EmployeesAndCompanies;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<Company, CompanyDTO>()
            .ForCtorParam("FullAddress", opt =>
                opt.MapFrom(x => $"{x.Address} {x.Country}"));
    }
}