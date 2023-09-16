using System.Windows;
using System.Windows.Controls;

namespace CorporateLogin.Wpf.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        //todo find alternative solution
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext != null) ((dynamic) DataContext).Password = ((PasswordBox) sender).SecurePassword;
        }
    }
}
