namespace Tecnyfarma.Server.Product.Domain;

public class User
{
    public Guid Id { get; }
    public string Email { get; }
    public UserType Type { get; }

    public User(string email, UserType type)
    {
        Id = Guid.NewGuid();
        Email = email;
        Type = type;
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