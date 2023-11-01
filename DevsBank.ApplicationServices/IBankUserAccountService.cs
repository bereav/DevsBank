using DevsBank.ApplicationServices.ReadModels;

namespace DevsBank.ApplicationServices;

public interface IBankUserAccountService
{
    public Task<Guid> OpenAccount(Guid customerId, int initialCredit);
    public Task<IEnumerable<AccountReadModel>> GetUserAccounts(Guid customerId);
}