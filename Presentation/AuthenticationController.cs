using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared;

namespace Presentation;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController: ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly IConfiguration _configuration;
    public AuthenticationController(IServiceManager serviceManager, IConfiguration configuration)
    {
        _serviceManager = serviceManager;
        _configuration =  configuration;
    }

    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
    {
        var result = await _serviceManager.AuthenticationService.RegisterUser(userForRegistrationDto);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        return StatusCode(201);
    }

    [HttpGet]
    public IActionResult GetKey()
    {
        var key = Environment.GetEnvironmentVariable("SECRET") ??
                  _configuration["JwtSettings:SecretKey"];
        return Ok(key);
    }
}