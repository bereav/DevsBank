using System.Diagnostics.CodeAnalysis;

namespace DevsBank.Storage;

public class StorageContext
{
    private readonly List<AccountEntity> _accounts = new();
    private readonly List<TransactionEntity> _transactions = new();
    private readonly BankUserEntity[] _bankUserEntities = new BankUserEntity[]
    {
        new()
        {
            Id = Guid.Parse("4886d666-32e6-4deb-b6e9-947f3334ca84"),
            Name = "Valentin",
            Surname = "Berea"
        },
        new()
        {
            Id = Guid.Parse("b7799a58-a34b-4d09-900a-e906238163ff"),
            Name = "Tudor",
            Surname = "Petrescu"
        },
    }.ToArray();

    // It looks strange having a Task here but the only reason I am doing it is to simulate a
    // database ORM that supports async I/O. This is valid for all other properties of this class.
    public Task<BankUserEntity[]> DbUsers => Task.FromResult(_bankUserEntities);

    public Task<IEnumerable<AccountEntity>> Accounts => Task.FromResult(_accounts.AsEnumerable());

    public Task<IEnumerable<TransactionEntity>> Transactions => Task.FromResult(_transactions.AsEnumerable());

    public void AddAccount([NotNull]AccountEntity account)
    {
        _accounts.Add(account);
    }

    public void AddTransaction([NotNull]TransactionEntity transaction)
    {
        _transactions.Add(transaction);
    }
}