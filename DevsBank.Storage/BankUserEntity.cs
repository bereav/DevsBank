namespace DevsBank.Storage;

/// <summary>
/// This class acts as a database entity. In realty this would be mapped to a real db storage.
/// The only reason I created it is to prove the point from a design perspective.
/// </summary>
public class BankUserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}