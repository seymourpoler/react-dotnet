namespace Tecnyfarma.Server.Product.Application.Product;

public class Args
{
    public string Email { get; private set; }
    
    public Args(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            Email = string.Empty;
            return;
        }

        Email = email;
    }
}