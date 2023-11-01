using Asp.Versioning;
using DevsBank.ApplicationServices;
using DevsBank.ApplicationServices.ReadModels;
using DevsBank.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DevsBank.WebApi.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly IBankUserService _bankUserService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IBankUserService bankUserService, ILogger<UsersController> logger)
    {
        _bankUserService = bankUserService;
        _logger = logger;
    }

    [HttpGet(Name = "[controller]/Get")]
    [Produces(typeof(UserReadModel))]
    public async Task<IEnumerable<UserReadModel>> Get()
    {
        var domainUsers= await _bankUserService.GetUsersAsync();
        return domainUsers.Select(domainUser => new UserReadModel
        {
            Id = domainUser.Id, Name = domainUser.Name, Surname = domainUser.Surname
        }).ToArray();
    }
}