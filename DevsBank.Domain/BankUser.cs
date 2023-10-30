namespace DevsBank.Domain;

public class BankUser
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    
    public BankUser(Guid id, string name, string surname)
    {
        if (id == Guid.Empty)
        {
            throw new InvalidOperationException("Id of the bank user was empty.");
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(surname))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(surname));
        }

        Id = id;
        Name = name;
        Surname = surname;
    }
}