namespace Tecnyfarma.Server.User.Application.SigIn;

public class UseCaseArgs
{
    public string Email {get; private set;}
    public string Password { get; private set; }
    
    public UseCaseArgs(string email, string password)
    {
        Email = email;
        Password = password;
    }
}