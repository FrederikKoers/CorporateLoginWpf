using CorporateLogin.Services.DbContext;
using System.Security;
using System.Text;
using CorporateLogin.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CorporateLogin.Services.DbServices
{
    public interface IUserService
    {
        User GetUserByName(string name);
        void CreateUser(string name, SecureString password);
        bool CheckPassword(string name, SecureString password);
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _users;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
            _users = context.Users;
        }
        
        public User GetUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.Username == name);
        }

        public void CreateUser(string name, SecureString password)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            try
            {
                char[] passwordChars = new char[password.Length];
                Marshal.Copy(ptr, passwordChars, 0, password.Length);

                var entity = new User { Username = name, PasswordHash = Passwordhasher.HashPasword(passwordChars) };
                _users.Add(entity);
                _context.SaveChanges();
                Array.Clear(passwordChars, 0, passwordChars.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        public bool CheckPassword(string name, SecureString password)
        {
            bool result = false;
            var user = GetUserByName(name);
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            try
            {
                char[] passwordChars = new char[password.Length];
                Marshal.Copy(ptr, passwordChars, 0, password.Length);

                result =  Passwordhasher.VerifyPassword(passwordChars, user.PasswordHash);
                Array.Clear(passwordChars, 0, passwordChars.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return result;
        }

    }
}

public static class Passwordhasher
{
    const int keySize = 64;
    const int iterations = 350000;
    private static readonly byte[] Salt;
    static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    static Passwordhasher()
    {
        Salt = RandomNumberGenerator.GetBytes(keySize);

        //Salt = Convert.FromBase64CharArray("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed dia".ToCharArray(),0, 64);
    }
    public static string HashPasword(char[] password)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            Salt,
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }

    public static bool VerifyPassword(char[] password, string hash)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Salt, iterations, hashAlgorithm, keySize);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}