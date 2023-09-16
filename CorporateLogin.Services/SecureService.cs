using CorporateLogin.Common.Models;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using CorporateLogin.Common.Interfaces;

namespace CorporateLogin.Services
{
    public class SecureService : ISecureService
    {
        //todo clean the marshal stuff
        public User CreateInitialUser(string name, SecureString password)
        {
            User user;
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            try
            {
                char[] passwordChars = new char[password.Length];
                Marshal.Copy(ptr, passwordChars, 0, password.Length);

                user = new User { Username = name, PasswordHash = PasswordHasher.HashPassword(passwordChars) };
                Array.Clear(passwordChars, 0, passwordChars.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return user;
        }

        public bool CheckPassword(User user, SecureString password)
        {
            bool result;
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            try
            {
                char[] passwordChars = new char[password.Length];
                Marshal.Copy(ptr, passwordChars, 0, password.Length);

                result = PasswordHasher.VerifyPassword(passwordChars, user.PasswordHash);
                Array.Clear(passwordChars, 0, passwordChars.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return result;
        }

        public bool CheckPasswordRules(SecureString password)
        {
            bool result = true;
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            try
            {
                char[] passwordChars = new char[password.Length];
                Marshal.Copy(ptr, passwordChars, 0, password.Length);

                //rules should be defined in DB, Institute or global, depending on the requirements  
                if (!passwordChars.Any(char.IsUpper))
                    result = false;
                
                if (!passwordChars.Any(char.IsLower))
                    result = false;
                
                if (!passwordChars.Any(char.IsDigit))
                    result = false;

                if (!passwordChars.Any(IsSpecialCharacter))
                    result = false;


                Array.Clear(passwordChars, 0, passwordChars.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return result;
        }

        private static bool IsSpecialCharacter(char c)
        {
            char[] specialCharacters = { '!', '@', '#', '$', '%', '^', '&', '*' };
            return specialCharacters.Contains(c);
        }

        private static class PasswordHasher
        {
            const int KeySize = 64;
            const int Iterations = 350000;
            private static readonly byte[] Salt;
            static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

            static PasswordHasher()
            {
                //should be generated once and stored in a secure place
                Salt = RandomNumberGenerator.GetBytes(KeySize);
            }
            public static string HashPassword(char[] password)
            {
                var hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    Salt,
                    Iterations,
                    HashAlgorithm,
                    KeySize);
                return Convert.ToHexString(hash);
            }

            public static bool VerifyPassword(char[] password, string hash)
            {
                var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Salt, Iterations, HashAlgorithm, KeySize);
                return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
            }
        }
    }
}
