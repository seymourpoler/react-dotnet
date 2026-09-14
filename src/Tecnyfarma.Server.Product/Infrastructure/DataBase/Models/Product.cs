namespace Tecnyfarma.Server.Product.Infrastructure.DataBase.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
}