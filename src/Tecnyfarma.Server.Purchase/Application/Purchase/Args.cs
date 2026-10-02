namespace Tecnyfarma.Server.Purchase.Application.Purchase;

public class Args
{
    public Args(Guid id, string? email)
    {
        Id = id;
        Email = email ?? string.Empty;
    }

    public string Email{ get; private set; }
    public Guid Id { get; private set; }
}