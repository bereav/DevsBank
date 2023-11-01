using DevsBank.ApplicationServices.ReadModels;
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

    public async Task<Guid> OpenAccount(Guid customerId, int initialCredit)
    {
        BankUser existingBankUser = await FindBankUser(customerId);
        var bankUserAccount = new BankUserAccount(Guid.NewGuid(), existingBankUser, _bankUserValidator);

        if (initialCredit > 0)
        {
            bankUserAccount.AddMoney(initialCredit);
        }

        SaveOpenAccountToDatabase(bankUserAccount, existingBankUser);
        return bankUserAccount.Id;// we might want to have something fancier than a GUID as Identity for an Account/BankUser...
    }

    public async Task<IEnumerable<AccountReadModel>> GetUserAccounts(Guid customerId)
    {
        List<AccountReadModel> readModel = new();
        BankUser existingBankUser = await FindBankUser(customerId);
        var userAccounts = (await _storageContext.Accounts)
            .Where(account => account.BankUserId == existingBankUser.Id);

        foreach (var accountEntity in userAccounts)
        {
            IEnumerable<TransactionReadModel> transactionReadModels = (await _storageContext.Transactions)
                .Where(t => t.AccountId == accountEntity.Id)
                .Select(transactionEntity => new TransactionReadModel
                {
                    SentAt = transactionEntity.SentAt,
                    TransactedAmount = transactionEntity.TransactedAmount,
                });

            readModel.Add(new AccountReadModel
            {
                Balance = accountEntity.Balance,
                Surname = existingBankUser.Surname,
                Name = existingBankUser.Name,
                Transactions = transactionReadModels
            });
        }

        return readModel.ToArray();
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
                AccountId = bankUserAccount.Id,
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