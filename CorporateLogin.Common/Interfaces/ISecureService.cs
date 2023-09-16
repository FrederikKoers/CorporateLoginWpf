using System.Security;
using CorporateLogin.Common.Models;

namespace CorporateLogin.Common.Interfaces;

public interface ISecureService
{
    User CreateInitialUser(string name, SecureString password);
    bool CheckPassword(User user, SecureString password);
    bool CheckPasswordRules(SecureString password);
}