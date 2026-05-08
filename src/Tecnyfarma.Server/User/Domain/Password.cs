using LanguageExt;

namespace Tecnyfarma.Server.User.Domain;

public class Password
{
     public string Value { get; private set; }
        
    private Password(string value)
    {
        Value = value; //hashing should be done here
    }
    
    public static Either<Error, Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            return new Error("Password must be at least 6 characters long");
        }
        
        return new Password(password);
    }
}