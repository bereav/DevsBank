namespace DevsBank.Domain.DomainValidators;

public interface IBankUserValidator
{
    public bool BankUserExists(BankUser bankUser);
}