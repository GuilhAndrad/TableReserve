using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using TableReserve.Domain.Security.PasswordHashing;

namespace TableReserve.Infrastructure.Security.PasswordHashing;

internal class Argon2PasswordHasher : IPasswordHasher
{
    private const int DegreeOfParallelism = 1;
    private const int Iterations = 2;
    private const int MemorySize = 20 * 1024; // 20 MB
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = HashPassword(password, salt);

        byte[] combinedBytes = new byte[hash.Length + salt.Length];

        salt.CopyTo(combinedBytes);
        hash.CopyTo(combinedBytes, index: salt.Length);

        return Convert.ToBase64String(combinedBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        byte[] combinedBytes = Convert.FromBase64String(passwordHash);

        byte[] salt = new byte[SaltSize];
        byte[] hash = new byte[HashSize];

        Array.Copy(combinedBytes, salt, SaltSize);
        Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);

        byte[] newHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(hash, newHash);
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        Argon2id hashAlgorithm = new(passwordBytes)
        {
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize,
            Salt = salt
        };

        return hashAlgorithm.GetBytes(HashSize);
    }
}