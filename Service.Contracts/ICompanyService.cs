using Shared;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges);
    CompanyDTO GetCompany(Guid companyId, bool trackChanges);
    CompanyDTO CreateCompany(CompanyForCreationDto company);
    IEnumerable<CompanyDTO> GetByIds(IEnumerable<Guid> companyIds, bool trackChanges);
    (IEnumerable<CompanyDTO> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies);
}