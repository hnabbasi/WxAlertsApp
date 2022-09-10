using System;
using System.Net.Http;
using System.Xml.Linq;
using XamWxApp.Models;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace XamWxApp.Services
{
	public class AlertService : IAlertService
	{
		private readonly HttpClient _httpClient;

		public AlertService()
		{
			_httpClient = GetHttpClient();
        }

		public async Task<Alert[]> GetAlerts(string stateCode)
        {
            var response = await _httpClient.GetStringAsync(new Uri($"/alerts/active/area/{stateCode.ToUpper()}"));
            return GetAlertsByState(JObject.Parse(response));
        }

        Alert[] GetAlertsByState(JObject json)
        {
            try
            {
                var features = json["features"];

                if (features == null || features.Type == JTokenType.Null || features.Count() == 0)
                {
                    Console.WriteLine($">>> No alerts found.");
                    return new Alert[0];
                }
                var count = features.Count();

                var alerts = new Alert[count];

                for (int i = 0; i < count; i++)
                {
                    var prop = features[i]?["properties"];

                    if (prop == null || prop.Type == JTokenType.Null)
                        return new Alert[0];

                    var alert = new Alert();

                    var id = prop["@id"];
                    if (id != null && id.Type != JTokenType.Null)
                        alert.Id = id.Value<string>();

                    var sent = prop["sent"];
                    if (sent != null && sent.Type != JTokenType.Null)
                        alert.Sent = DateTimeOffset.Parse(sent.Value<string>());

                    var @event = prop["event"];
                    if (@event != null && @event.Type != JTokenType.Null)
                        alert.Event = @event.Value<string>();

                    var headline = prop["headline"];
                    if (headline != null && headline.Type != JTokenType.Null)
                        alert.Headline = headline.Value<string>();

                    alerts[i] = alert;
                }
                Console.WriteLine($">>> Found {count} alerts");
                return alerts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Alert[0];
            }
        }

        HttpClient GetHttpClient()
		{
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.weather.gov");
            httpClient.DefaultRequestHeaders.Add("User-agent", "wxapp");
            return httpClient;
        }
	}
}

