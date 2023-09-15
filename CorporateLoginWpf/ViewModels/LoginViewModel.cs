using Prism.Commands;
using Prism.Mvvm;
using System.Security;
using System.Windows;

namespace CorporateLoginWpf.ViewModels
{
    public class LoginViewModel : BindableBase
    {
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

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            if (IsValidLogin(Username, Password))
            {
                MessageBox.Show("Anmeldung erfolgreich!");
                
            }
            else
            {
                MessageBox.Show("Falscher Benutzername oder Passwort.");
            }
        }

        private bool IsValidLogin(string username, SecureString password)
        {
            // Hier sollten Sie die Datenbankabfrage für die Benutzerdaten implementieren.
            // Überprüfen Sie auch, ob das Passwort den Sicherheitsrichtlinien entspricht.
            // Wenn der Benutzer erfolgreich authentifiziert wird, geben Sie true zurück, sonst false.
            return false; // Implementieren Sie Ihre Logik hier.
        }
    }
}
