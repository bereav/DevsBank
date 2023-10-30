namespace DevsBank.Storage;

public class StorageContext
{
    public Task<BankUserEntity[]> DbUsers
    {
        get
        {
            var bankUserEntities = new BankUserEntity[]
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
            
            return Task.FromResult(bankUserEntities);
        }
    }
}