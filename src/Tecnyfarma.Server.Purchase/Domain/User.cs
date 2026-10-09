using LanguageExt;

namespace Tecnyfarma.Server.Purchase.Domain;

public class User
{
    public Guid Id { get; }
    public string Email { get; }
    public UserType Type { get; }

    private User(string email, UserType type)
    {
        Id = Guid.NewGuid();
        Email = email;
        Type = type;
    }

    public static Either<Error, User> Create(string email, UserType type)
    {
        return new User(email, type);
    }
    
    public float CalculatePrice(float price)
    {
        return Type switch
        {
            UserType.Freemium => price,
            UserType.Premium => price * 0.9f,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}