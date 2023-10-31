namespace DevsBank.Domain;

public class BankAccountTransaction
{
    public int TransactedAmount { get; }
    public DateTime SentAt { get; }

    public BankAccountTransaction(int transactedAmount)
    {
        // A transaction should normally have more details than this.
        // For example which user is the author of the transaction.
        // It might be the owner of the account but it might also be another bank user as well.
        TransactedAmount = transactedAmount;
        SentAt = DateTime.UtcNow;
    }
}