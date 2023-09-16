using System.Security;

namespace CorporateLogin.Common.Interfaces;

public interface IUserService
{
    bool Login(string name, SecureString password);
    bool CreateUser(string username, SecureString password);
}