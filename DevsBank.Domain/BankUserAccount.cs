using DevsBank.Domain.DomainValidators;

namespace DevsBank.Domain;

public class BankUserAccount
{
    private readonly BankUser _existingBankUser;
    private readonly List<BankAccountTransaction> _transactions;

    public Guid Id { get; }

    // I know that money is not an integer and for very rich people it won't be enough
    // but money can be represented in different ways in the code so
    // for the sake of simplicity DevsBank will only accept money as integers. What's a cent worth these days?
    public int Balance { get; private set; } = 0;
    public IReadOnlyCollection<BankAccountTransaction> Transactions => _transactions.AsReadOnly();
    public string Name => _existingBankUser.Name;
    public string SurName => _existingBankUser.Surname;

    public BankUserAccount(Guid id, BankUser existingBankUser, IBankUserValidator bankUserValidator)
    {
        if (!bankUserValidator.BankUserExists(existingBankUser))
        {
            throw new InvalidOperationException("Unknown bank account user");
        }

        Id = id;
        _existingBankUser = existingBankUser;
        _transactions = new List<BankAccountTransaction>();
    }

    public void AddMoney(int money)
    {
        Balance += money;
        _transactions.Add(new BankAccountTransaction(money));
    }
}