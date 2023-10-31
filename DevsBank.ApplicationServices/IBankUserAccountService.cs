namespace DevsBank.ApplicationServices;

public interface IBankUserAccountService
{
    public Task OpenAccount(Guid customerId, int initialCredit);
}