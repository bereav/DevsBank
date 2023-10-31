namespace DevsBank.ApplicationServices;

public interface IBankUserAccountService
{
    public Task<Guid> OpenAccount(Guid customerId, int initialCredit);
}