using System;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json;
using XamWxApp.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace XamWxApp
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public MainPage()
        {
            InitializeComponent();
            _httpClient = GetHttpClient();
        }

        async void GetAlertsButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Reset();

            var response = await _httpClient.GetStringAsync(new Uri($"https://api.weather.gov/alerts/active/area/{StateCode.Text.ToUpper()}"));
            var alerts = GetAlertsByZones(JObject.Parse(response));

            UpdateUI(alerts);
        }

        void Reset()
        {
            AlertsCount.Text = "";
            AlertsList.ItemsSource = null;
        }

        HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-agent", "wxapp");
            return httpClient;
        }

        void UpdateUI(Alert[] alerts)
        {
            if (alerts.Length > 0)
                AlertsCount.Text = $"Found {alerts.Length} active alerts";
            else
                AlertsCount.Text = "No active alerts found";

            AlertsList.ItemsSource = alerts;
        }

        Alert[] GetAlertsByZones(JObject json)
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
#if DEBUG
                Console.WriteLine($">>> Parsing {features.Count()} alerts...");
#endif

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

        ~MainPage()
        {
            _httpClient.Dispose();
        }
    }
}

