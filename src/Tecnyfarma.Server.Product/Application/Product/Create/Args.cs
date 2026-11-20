namespace Tecnyfarma.Server.Product.Application.Product.Create;

public class Args
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public float Price { get; private set; }
    
    public Args(string name, string description, float price)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}