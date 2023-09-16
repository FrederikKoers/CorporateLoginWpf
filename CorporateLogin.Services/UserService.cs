using CorporateLogin.Common.Models;
using CorporateLogin.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security;
using CorporateLogin.Services.DbServices;

namespace CorporateLogin.Services
{

    public interface IUserService
    {
        bool Login(string name, SecureString password);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecureService _secureService;

        public UserService(IUserRepository userRepository, ISecureService secureService)
        {
            _userRepository = userRepository;
            _secureService = secureService;
        }


        public bool Login(string name, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                //validation name is empty
                return false;
            }

            //Password rules need to be defined
            if (password.Length > 3)
            {
                //validation password is empty or too short
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

        private void IncreaseFailedPasswordAttempts(User user)
        {
            user.FailedPasswordAttempts++;
            if (user.FailedPasswordAttempts >= 3)
            {
                user.Blocked = true;
            }

            _userRepository.Update(user);
        }
    }
}