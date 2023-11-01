namespace DevsBank.ApplicationServices.ReadModels;

public class AccountReadModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Balance { get; set; }
    public IEnumerable<TransactionReadModel> Transactions { get; set; }
}