namespace Tecnyfarma.Server.User.Application.Register;

public class Args
{
    public string Email { get; }
    public string Password { get; }
    
    public Args(string email, string password)
    {
        Email = email;
        Password = password;
    }
}