using LanguageExt;

namespace Tecnyfarma.Server.User.Domain;

public class Email
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }
    
    public static Either<Error, Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            return new Error("Invalid email");
        }
        
        return new Email(email);
    }

    public bool IsEqualTo(Email other)
    {
        return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}