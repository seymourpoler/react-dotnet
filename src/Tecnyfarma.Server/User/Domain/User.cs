namespace Tecnyfarma.Server.User.Domain;

public class User
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    public User(Email email, Password password)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
    }
}