namespace Tecnyfarma.Server.Purchase.Application.Product;

public class Args
{
    public Guid IdProduct { get; private set; }
    public string Name { get; private set; }
    public float Price { get; private set; }
    
    public Args(Guid idProduct, string name, float price)
    {
        IdProduct = idProduct;
        Name = name;
        Price = price;
    }
}