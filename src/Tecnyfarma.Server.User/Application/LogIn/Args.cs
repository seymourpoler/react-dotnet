namespace Tecnyfarma.Server.User.Application.LogIn;

public class Args
{
    public string Email {get; private set;}
    public string Password { get; private set; }
    
    public Args(string email, string password)
    {
        Email = email;
        Password = password;
    }
}