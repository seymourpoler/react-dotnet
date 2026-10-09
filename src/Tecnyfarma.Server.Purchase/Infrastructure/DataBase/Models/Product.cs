namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
}