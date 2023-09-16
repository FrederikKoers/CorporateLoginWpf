using System.Security;
using System.Windows;
using CorporateLogin.Common.Interfaces;
using CorporateLogin.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace CorporateLogin.Wpf.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IUserService _userService;

        private string _username;
        private SecureString _password;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public SecureString Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand CreateUserCommand { get; }

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            LoginCommand = new DelegateCommand(Login);
            CreateUserCommand = new DelegateCommand(CreateUser);
        }



        private void Login()
        {
            if (IsValidLogin(Username, Password))
            {
                MessageBox.Show("Anmeldung erfolgreich!");
                
            }
            else
            {
                //proper Validation Steps
                MessageBox.Show("Falscher Benutzername oder Passwort.");
            }
        }

        private bool IsValidLogin(string username, SecureString password)
        {
            return _userService.Login(username, password);
        }


        private void CreateUser()
        {
            if (_userService.CreateUser(Username, Password))
            {
                MessageBox.Show("Benutzer angelegt!");

            }
            else
            {
                //proper Validation Steps
                MessageBox.Show("Benutzer konnte nicht angelegt werden.");
            }
        }
    }
}
