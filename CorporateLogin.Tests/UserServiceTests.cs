using System.Security;
using CorporateLogin.Common.Interfaces;
using CorporateLogin.Common.Models;
using CorporateLogin.Services;
using CorporateLogin.Services.Repository;
using CorporateLogin.Tests.Helpers;
using Moq;
using Xunit;

namespace CorporateLogin.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ISecureService> _secureService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _secureService = new Mock<ISecureService>();
            _userService = new UserService(_userRepository.Object, _secureService.Object);
        }

        [Fact]
        public void LoginWithValidUser()
        {
            var username = "JohnDoe";
            var user = new User
            {
                Username = username,
                Blocked = false,
                Verified = true
            };
            var password = SecureStringHelper.CreateSecureString("1aB!");

            _userRepository.Setup(repo => repo.GetUserByName(username)).Returns(user);
            _secureService.Setup(service => service.CheckPassword(user, password)).Returns(true);
            

            var result = _userService.Login(username, password);


            Assert.True(result);
        }

        [Fact]
        public void LoginUnknownUser()
        {
            _userRepository.Setup(repo => repo.GetUserByName(It.IsAny<string>())).Returns((User)null);
            
            var result = _userService.Login("NonExistentUser", SecureStringHelper.CreateSecureString("1aB!"));
            
            Assert.False(result);
        }

        [Fact]
        public void CreateNewUser()
        {
            _userRepository.Setup(repo => repo.CheckUserExistByName("NewUser")).Returns(false);
            _secureService.Setup(service => service.CheckPasswordRules(It.IsAny<SecureString>())).Returns(true);
            _secureService.Setup(service => service.CreateInitialUser("NewUser", It.IsAny<SecureString>())).Returns(new User());


            var result = _userService.CreateUser("NewUser", SecureStringHelper.CreateSecureString("1aB!"));


            Assert.True(result);
        }

        //Add all Validation Tests
    }
}