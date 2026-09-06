namespace Tecnyfarma.Server.Product.Application;

public class Args
{
    public string Email { get; private set; }
    
    public Args(string email)
    {
        Email = email;
    }
}