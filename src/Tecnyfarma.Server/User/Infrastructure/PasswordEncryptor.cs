using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Tecnyfarma.Server.User.Infrastructure;

public static class PasswordEncryptor
{
    public static string Encrypt(string password)
    {
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: password!,
                salt: RandomNumberGenerator.GetBytes(128 / 8),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
        );
    } 
}