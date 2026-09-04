using LanguageExt;
using Tecnyfarma.Server.User.Infrastructure;

namespace Tecnyfarma.Server.User.Domain;

public class Password
{
     public string Value { get; private set; }
        
    private Password(string value)
    {
        Value = PasswordEncryptor.Encrypt(value);
    }
    
    public static Either<Error, Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            return new Error("Password must be at least 6 characters long");
        }
        
        return new Password(password);
    }

    public bool IsEqualTo(Password other)
    {
        return  Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}