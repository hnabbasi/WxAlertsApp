using System;
using Prism.Navigation;

namespace XamWxApp.ViewModels
{
    public class BaseViewModel : Prism.Mvvm.BindableBase, Prism.Navigation.IInitialize
    {
        private string _title;

        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public BaseViewModel()
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }
    }
}

