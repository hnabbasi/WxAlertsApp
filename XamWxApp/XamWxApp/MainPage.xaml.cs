using System;
using System.Net.Http;
using Xamarin.Forms;
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

        HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "wx-app");
            return httpClient;
        }

        async void GetAlertsButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Reset();

            var response = await _httpClient.GetStringAsync(new Uri($"https://api.weather.gov/alerts/active/area/{StateCode.Text.ToUpper()}"));
            var alerts = GetAlertsByZones(JObject.Parse(response));

            UpdateUi(alerts);
        }

        void Reset()
        {
            AlertsCount.Text = "";
            AlertsList.ItemsSource = null;
        }

        void UpdateUi(Alert[] alerts)
        {
            AlertsCount.Text = alerts.Length > 0 
                ? $"Found {alerts.Length} active alerts" 
                : "No active alerts found";
            AlertsList.ItemsSource = alerts;
        }

        Alert[] GetAlertsByZones(JObject json)
        {
            try
            {
                var features = json["features"];

                if (features == null || features.Type == JTokenType.Null || features.Count() == 0)
                    return Array.Empty<Alert>();
                
                var count = features.Count();

                var alerts = new Alert[count];

                for (var i = 0; i < count; i++)
                {
                    var prop = features[i]?["properties"];

                    if (prop == null || prop.Type == JTokenType.Null)
                        return Array.Empty<Alert>();

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
                return alerts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Array.Empty<Alert>();
            }
        }

        ~MainPage()
        {
            _httpClient.Dispose();
        }
    }
}