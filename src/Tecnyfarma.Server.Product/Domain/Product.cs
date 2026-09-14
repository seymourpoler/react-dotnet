namespace Tecnyfarma.Server.Product.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public float Price { get; private set; }

    public Product(Guid id, string name, string description, float price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }
}