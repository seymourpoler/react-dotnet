namespace Tecnyfarma.Server.Product.Message;

public class ProductCreated
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
}