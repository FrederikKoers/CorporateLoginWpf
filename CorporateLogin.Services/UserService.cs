using CorporateLogin.Common.Models;
using CorporateLogin.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security;
using System.Xml.Linq;

namespace CorporateLogin.Services
{

    public interface IUserService
    {
        bool Login(string name, SecureString password);
        bool CreateUser(string username, SecureString password);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecureService _secureService;
        private const int MaxFailedPasswordAttempts = 3;
        private const int PasswordMinimumLength = 3;

        public UserService(IUserRepository userRepository, ISecureService secureService)
        {
            _userRepository = userRepository;
            _secureService = secureService;
        }

        private bool ValidateUserAndPasswort(string name, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                //validation name is empty
                return false;
            }

            //Password rules need to be defined
            if (password.Length > PasswordMinimumLength)
            {
                //validation password is empty or too short
                return false;
            }

            return true;
        }   


        public bool Login(string name, SecureString password)
        {
            if (ValidateUserAndPasswort(name, password))
            {
                //validation name or password is empty or too short
                return false;
            }

            if (_userRepository.GetUserByName(name) is {} user)
            {
                if (user.Blocked)
                {
                    //Validation User is blocked
                    return false;
                }

                if (!user.Verified)
                {
                    //Not verified by admin
                    return false;
                }

                if (!_secureService.CheckPassword(user, password))
                {
                    IncreaseFailedPasswordAttempts(user);
                    //Validation wrong password
                    return false;
                }
                
                return true;
            }

            //Validation user not found
            return false;
        }

        public bool CreateUser(string username, SecureString password)
        {
            if (ValidateUserAndPasswort(username, password))
            {
                //validation name or password is empty or too short
                return false;
            }

            if (_userRepository.CheckUserExistByName(username))
            {
                //Validation user already exists
                return false;
            }

            if (!_secureService.CheckPasswordRules(password))
            {
                //Validation password rules
                return false;
            }

            var newUser = _secureService.CreateInitialUser(username, password);
#if DEBUG
            newUser.Verified = true;
#endif
            _userRepository.Update(newUser);



            return true;

        }

        private void IncreaseFailedPasswordAttempts(User user)
        {
            user.FailedPasswordAttempts++;
            if (user.FailedPasswordAttempts >= MaxFailedPasswordAttempts)
            {
                user.Blocked = true;
            }

            _userRepository.Update(user);
        }
    }
}