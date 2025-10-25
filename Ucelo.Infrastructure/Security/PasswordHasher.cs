using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Ucelo.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    public (string passwordHash, string saltHash) Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Senha não pode ser vazia", nameof(password));

        var salt = GenerateSalt();
        var hash = GenerateHash(password, salt);

        var saltHash = Convert.ToBase64String(salt);
        var passwordHash = Convert.ToBase64String(hash);

        return (passwordHash, saltHash);
    }

    public bool Verify(string password, string passwordHash, string saltHash)
    {
        if (string.IsNullOrWhiteSpace(password) || 
            string.IsNullOrWhiteSpace(passwordHash) || 
            string.IsNullOrWhiteSpace(saltHash))
            return false;

        try
        {
            var salt = Convert.FromBase64String(saltHash);
            var hash = Convert.FromBase64String(passwordHash);
            var testHash = GenerateHash(password, salt);

            return CryptographicOperations.FixedTimeEquals(hash, testHash);
        }
        catch
        {
            return false;
        }
    }

    private byte[] GenerateSalt()
    {
        var salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private byte[] GenerateHash(string password, byte[] salt)
    {
        return KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize);
    }
}