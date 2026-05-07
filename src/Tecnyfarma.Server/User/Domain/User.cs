namespace Tecnyfarma.Server.User.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
    }
}