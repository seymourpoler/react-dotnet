namespace Tecnyfarma.Server.User.Application.Login;

public class UseCaseArgs
{
    public string Email { get; }
    public string Password { get; }

    public UseCaseArgs(string email, string password)
    {
        Email = email;
        Password = password;
    }
}