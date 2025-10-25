namespace Ucelo.Infrastructure.Security;

public interface IPasswordHasher
{
    (string passwordHash, string saltHash) Hash(string password);
    bool Verify(string password, string passwordHash, string saltHash);
}