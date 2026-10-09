using LanguageExt;

namespace Tecnyfarma.Server.Purchase.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public float Price { get; private set; }
    
    private Product(Guid id, string name, float price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static Either<Error, Product> Create(Guid id, string name, float price)
    {
        return new Product(id, name, price);
    }
}