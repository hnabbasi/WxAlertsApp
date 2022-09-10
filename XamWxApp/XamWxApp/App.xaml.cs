using System;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamWxApp.Services;
using XamWxApp.ViewModels;
using XamWxApp.Views;

namespace XamWxApp
{
    public partial class App : PrismApplication
    {
        public App() : this(null)
        {

        }

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer)
        {

        }

        protected async override void OnInitialized()
        {
            await NavigationService.NavigateAsync(nameof(Views.MainPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Pages
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            // Services
            containerRegistry.Register<IAlertService, AlertService>();
        }
    }
}

