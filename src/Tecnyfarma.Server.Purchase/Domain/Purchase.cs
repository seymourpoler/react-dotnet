using LanguageExt;

namespace Tecnyfarma.Server.Purchase.Domain;

public class Purchase
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public Guid IdProduct { get; private set; }
    public string ProductProductName { get; private set; } 
    public float Price { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Purchase(string email, Guid idProduct, string productName, float price)
    {
        Id = Guid.NewGuid();
        Email = email;
        IdProduct = idProduct;
        ProductProductName = productName;
        Price = price;
        CreatedAtUtc = DateTime.UtcNow;
    }
    
    public static Either<Error, Purchase> Create(User user, Product product)
    {
        return new Purchase(user.Email, product.Id, product.Name, user.CalculatePrice(product.Price));
    }
}