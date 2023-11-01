using DevsBank.Domain;

namespace DevsBank.ApplicationServices;

public interface IBankUserService
{
    public Task<IEnumerable<BankUser>> GetUsersAsync();
}