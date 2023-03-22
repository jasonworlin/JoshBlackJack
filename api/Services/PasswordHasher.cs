using System;
using System.Security.Cryptography;
using System.Text;
namespace api.Services;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashedPassword);
}

public class PasswordHasher : IPasswordHasher
{
    private static readonly int saltSize = 16;

    public string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = GenerateSalt();

        // Convert the password string to a byte array
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Create a new byte array to hold the salt and password bytes
        byte[] hashBytes = new byte[saltSize + passwordBytes.Length];

        // Copy the salt and password bytes into the new array
        Array.Copy(salt, 0, hashBytes, 0, saltSize);
        Array.Copy(passwordBytes, 0, hashBytes, saltSize, passwordBytes.Length);

        // Compute the hash of the salt and password bytes using the SHA256 algorithm
        HashAlgorithm hashAlgorithm = new SHA256Managed();
        byte[] hash = hashAlgorithm.ComputeHash(hashBytes);

        // Create a new byte array to hold the salt and hash bytes
        byte[] hashWithSaltBytes = new byte[saltSize + hash.Length];

        // Copy the salt and hash bytes into the new array
        Array.Copy(salt, 0, hashWithSaltBytes, 0, saltSize);
        Array.Copy(hash, 0, hashWithSaltBytes, saltSize, hash.Length);

        // Convert the byte array to a base64-encoded string and return it
        return Convert.ToBase64String(hashWithSaltBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Convert the hashed password from a base64-encoded string to a byte array
        byte[] hashWithSaltBytes = Convert.FromBase64String(hashedPassword);

        // Extract the salt from the hashed password byte array
        byte[] salt = new byte[saltSize];
        Array.Copy(hashWithSaltBytes, 0, salt, 0, saltSize);

        // Convert the password string to a byte array
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Create a new byte array to hold the salt and password bytes
        byte[] hashBytes = new byte[saltSize + passwordBytes.Length];

        // Copy the salt and password bytes into the new array
        Array.Copy(salt, 0, hashBytes, 0, saltSize);
        Array.Copy(passwordBytes, 0, hashBytes, saltSize, passwordBytes.Length);

        // Compute the hash of the salt and password bytes using the SHA256 algorithm
        HashAlgorithm hashAlgorithm = new SHA256Managed();
        byte[] hash = hashAlgorithm.ComputeHash(hashBytes);

        // Compare the computed hash with the hash from the hashed password byte array
        for (int i = 0; i < hash.Length; i++)
        {
            if (hash[i] != hashWithSaltBytes[saltSize + i])
            {
                // If any byte of the computed hash differs from the corresponding byte in the hashed password, return false
                return false;
            }
        }

        // If all bytes of the computed hash match the corresponding bytes in the hashed password, return true
        return true;
    }

    private static byte[] GenerateSalt()
    {
        // Create a new byte array to hold the salt
        byte[] salt = new byte[saltSize];

        // Create a new instance of RNGCryptoServiceProvider to generate random bytes
        using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
        {
            // Generate random bytes and store them in the salt array
            rngCsp.GetBytes(salt);
        }

        // Return the salt array
        return salt;
    }
}
