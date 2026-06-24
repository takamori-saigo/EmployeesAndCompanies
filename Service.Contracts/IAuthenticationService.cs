using Microsoft.AspNetCore.Identity;
using Shared;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto user);
}