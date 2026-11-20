namespace Tecnyfarma.Server.Purchase.Message;

public class PurchaseCreated
{
    public string Email { get; set; }
    public string Name {get; set;}
    public float Price {get; set;}
    public DateTime CreatedAtUtc { get;  set; }
}