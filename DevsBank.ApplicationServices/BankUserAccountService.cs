using DevsBank.Domain;
using DevsBank.Domain.DomainValidators;
using DevsBank.Storage;

namespace DevsBank.ApplicationServices;

public class BankUserAccountService : IBankUserAccountService
{
    private readonly IBankUserValidator _bankUserValidator;
    private readonly StorageContext _storageContext;

    public BankUserAccountService(IBankUserValidator bankUserValidator, StorageContext storageContext)
    {
        _bankUserValidator = bankUserValidator;
        _storageContext = storageContext;
    }

    public async Task OpenAccount(Guid customerId, int initialCredit)
    {
        BankUser existingBankUser = await FindBankUser(customerId);
        var bankUserAccount = new BankUserAccount(Guid.NewGuid(), existingBankUser, _bankUserValidator);

        if (initialCredit > 0)
        {
            bankUserAccount.AddMoney(initialCredit);
        }

        SaveOpenAccountToDatabase(bankUserAccount, existingBankUser);
    }

    private void SaveOpenAccountToDatabase(BankUserAccount bankUserAccount, BankUser existingBankUser)
    {
        _storageContext.AddAccount(new AccountEntity
        {
            Id = bankUserAccount.Id,
            Balance = bankUserAccount.Balance,
            BankUserId = existingBankUser.Id,
        });

        foreach (var transaction in bankUserAccount.Transactions)
        {
            _storageContext.AddTransaction(new TransactionEntity
            {
                TransactedAmount = transaction.TransactedAmount,
                SentAt = transaction.SentAt
            });
        }

    }

    private async Task<BankUser> FindBankUser(Guid customerId)
    {
        var bankUserEntity = (await _storageContext.DbUsers).SingleOrDefault(user => user.Id == customerId);
        if (bankUserEntity == null)
        {
            // an actual domain exception should be thrown here something like UserUnkownException.
            throw new InvalidOperationException($"The user with id {customerId} does not exist");
        }

        var existingBankUser = new BankUser(bankUserEntity.Id, bankUserEntity.Name, bankUserEntity.Surname);
        return existingBankUser;
    }
}