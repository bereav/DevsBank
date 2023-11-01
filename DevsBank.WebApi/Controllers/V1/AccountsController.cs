using Asp.Versioning;
using DevsBank.ApplicationServices;
using DevsBank.ApplicationServices.ReadModels;
using DevsBank.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DevsBank.WebApi.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AccountsController : ControllerBase
{
    private readonly IBankUserAccountService _bankAccountService;

    public AccountsController(IBankUserAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    [HttpPost(Name = "[controller]/OpenAccount")]
    public async Task<ActionResult<Guid>> OpenAccount(Guid customerId, int credit)
    {
        Guid bankUserAccountId = await _bankAccountService.OpenAccount(customerId, credit);

        return new OkObjectResult(bankUserAccountId);
    }

    [HttpGet(Name = "[controller]/GetAccounts")]
    public async Task<ActionResult<IEnumerable<AccountReadModel>>> GetAccounts(Guid customerId)
    {
        IEnumerable<AccountReadModel> accounts = await _bankAccountService.GetUserAccounts(customerId);

        return new OkObjectResult(accounts);
    }
}