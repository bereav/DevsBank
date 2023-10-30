using DevsBank.Domain;
using DevsBank.Storage;

namespace DevsBank.ApplicationServices;

public class BankUserService : IBankUserService
{
    private readonly StorageContext _storageContext;

    public BankUserService(StorageContext storageContext)
    {
        _storageContext = storageContext;
    }
    
    public async Task<IEnumerable<BankUser>> GetUsersAsync()
    {
        // just for showing the design we create a domain class here, BankUser.
        // since there is no real database with async I/O implementation await is not really needed but it
        // is here to prove how this would be done in real system.
        var dbUsers = await _storageContext.DbUsers;
        return dbUsers.Select(entity => new BankUser(entity.Id, entity.Name, entity.Surname)).ToArray();
    }
}