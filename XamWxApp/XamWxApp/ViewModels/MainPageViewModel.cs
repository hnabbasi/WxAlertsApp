using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using XamWxApp.Models;
using XamWxApp.Services;

namespace XamWxApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;

        public ICommand GetAlertsCommand { get; set; }

        private string _stateCode;
        public string StateCode
        {
            get => _stateCode;
            set => SetProperty(ref _stateCode, value);
        }

        private string _alertsCount;
        public string AlertsCount
        {
            get => _alertsCount;
            set => SetProperty(ref _alertsCount, value);
        }

        private Alert[] _alerts;
        public Alert[] Alerts
        {
            get => _alerts;
            set => SetProperty(ref _alerts, value);
        }

        public MainPageViewModel(IAlertService alertService)
        {
            Title = "Weather Alerts";
            _alertService = alertService;

            GetAlertsCommand = new DelegateCommand(OnGetAlertsTapped);
        }

        private async void OnGetAlertsTapped()
        {
            Alerts = await _alertService.GetAlerts(StateCode);

            if (Alerts.Length > 0)
                AlertsCount = $"Found {Alerts.Length} active alerts";
            else
                AlertsCount = "No active alerts found";
        }
    }
}

