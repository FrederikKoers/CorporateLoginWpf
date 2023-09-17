using System.Windows;
using CorporateLogin.Common.Interfaces;
using CorporateLogin.Services;
using CorporateLogin.Services.Repository;
using CorporateLogin.Wpf.Views;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;

namespace CorporateLogin.Wpf
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<LoginView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");

            containerRegistry.RegisterInstance(optionsBuilder.Options);
            
            containerRegistry.RegisterSingleton<IUserRepository, UserRepository>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<ISecureService, SecureService>();

        }
    }
}
