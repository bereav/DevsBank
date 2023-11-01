namespace DevsBank.Domain.DomainValidators;

public interface IBankUserValidator
{
    public bool BankUserExists(Guid userIdentity);
    public bool BankUserExists(BankUser bankUser);
}