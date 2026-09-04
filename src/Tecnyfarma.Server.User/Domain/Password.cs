using LanguageExt;
using Tecnyfarma.Server.User.Infrastructure;

namespace Tecnyfarma.Server.User.Domain;

public class Password
{
     public string Value { get; private set; }
        
    private Password(string value)
    {
        Value = value;
    }
    
    public static Either<Error, Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            return new Error("Password must be at least 6 characters long");
        }
        
        var encryptedPassword = PasswordEncryptor.Encrypt(password);
        return new Password(encryptedPassword);
    }

    public static Either<Error, Password> CreateWithEncryptedValue(string value)
    {
        return new Password(value);
    }
    
    public bool IsEqualTo(Password other)
    {
        return  Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}