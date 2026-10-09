namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase.Models;

public class Purchase
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Guid ProductId { get; set; }
    public float Price { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}