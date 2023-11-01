using DevsBank.Domain;
using DevsBank.Domain.DomainValidators;
using DevsBank.Storage;

namespace DevsBank.ApplicationServices.Validators;

public class BankUserValidator : IBankUserValidator
{
    private readonly StorageContext _storageContext;

    public BankUserValidator(StorageContext storageContext)
    {
        _storageContext = storageContext;
    }

    public bool BankUserExists(Guid userIdentity)
    {
        return UserExists(userIdentity);
    }

    public bool BankUserExists(BankUser bankUser)
    {
        return UserExists(bankUser.Id);
    }

    private bool UserExists(Guid bankUserId)
    {
        return _storageContext.DbUsers.Result.Any(user => user.Id == bankUserId);
    }
}