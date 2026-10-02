namespace Tecnyfarma.Server.Purchase.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public float Price { get; private set; }
    
    public Product(Guid id, string name, float price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}