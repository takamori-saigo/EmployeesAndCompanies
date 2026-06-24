using AutoMapper;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared;

namespace Service;

internal sealed class AuthenticationService: IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto usesForRegistration)
    {
        var user = _mapper.Map<User>(usesForRegistration);
        var result = await _userManager.CreateAsync(user, usesForRegistration.Password);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, usesForRegistration.Roles);
        return result;
    }
}