namespace DevsBank.Storage;

public class TransactionEntity
{
    public Guid AccountId { get; set; }
    public int TransactedAmount { get; set; }
    public DateTime SentAt { get; set; }
}