using LanguageExt;

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

    public static Either<Error, Product> Create(string name, string description, float price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new Error("Product name is required");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return new Error("Product description is required");
        }

        if (price <= 0)
        {
            return new Error("Product price must be greater than zero");
        }
        return new Product(Guid.NewGuid(), name, description, price);
    }
}