﻿using CorporateLoginWpf.Views;
using Prism.Ioc;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using CorporateLogin.Services;
using CorporateLogin.Services.DbServices;
using Microsoft.Extensions.Options;
using CorporateLogin.Services.Repository;

namespace CorporateLoginWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
