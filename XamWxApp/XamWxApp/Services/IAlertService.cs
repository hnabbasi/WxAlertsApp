using System;
using System.Threading.Tasks;
using XamWxApp.Models;

namespace XamWxApp.Services
{
	public interface IAlertService
	{
        Task<Alert[]> GetAlerts(string stateCode);
    }
}

