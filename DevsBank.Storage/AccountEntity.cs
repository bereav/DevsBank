namespace DevsBank.Storage;

public class AccountEntity
{
    public Guid Id { get; set; }
    public Guid BankUserId { get; set; }
    public int Balance { get; set; }
}