namespace Tecnyfarma.Server.User.Domain;

public class Purchase
{
    public Guid Id { get; private set; }
    public string Email{ get; private set; }
    public string Name { get; private set; }
    public float Price { get; private set; }
    public DateTime CreatedAtUtc{ get; private set; }
    
    public Purchase(string email, string name, float price, DateTime createdAtUtc)
    {
        Id   = Guid.NewGuid();
        Email = email;
        Name = name;
        Price = price;
        CreatedAtUtc = createdAtUtc;
    }
}