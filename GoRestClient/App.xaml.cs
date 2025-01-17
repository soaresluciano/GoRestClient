﻿using GoRestClient.Core;
using GoRestClient.Services;
using GoRestClient.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace GoRestClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .Register<IConfigurationProvider, ConfigurationProvider>()
                .Register<IRestProvider, RestProvider>()
                .Register<IUserService, UserService>()
                .Register<IStatusManager, StatusManager>()
                .Register<IJsonProvider, JsonProvider>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}
