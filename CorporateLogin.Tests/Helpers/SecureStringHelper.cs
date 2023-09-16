using System.Security;

namespace CorporateLogin.Tests.Helpers
{
    internal static class SecureStringHelper
    {

        internal static SecureString CreateSecureString(string password)
        {
            var secureString = new SecureString();
            foreach (var c in password)
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }
    }
}
