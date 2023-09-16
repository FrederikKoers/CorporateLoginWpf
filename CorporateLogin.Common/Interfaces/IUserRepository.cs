using CorporateLogin.Common.Models;

namespace CorporateLogin.Common.Interfaces;

public interface IUserRepository
{
    User GetUserByName(string name);
    User Update(User user);
    bool CheckUserExistByName(string name);
}