using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using WeatherApp.Interfaces;
using WeatherApp.MVVM.Models;

namespace WeatherApp.MVVM.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
        #region Variables

        [ObservableProperty]
        private IWeatherData? _weatherData;
		[ObservableProperty]
		private IGeoData[]? _geoDataList;
		[ObservableProperty]
		private bool _isVisible_GeoDataList;
		[ObservableProperty]
		private IGeoData? _selectedLocation;
		[ObservableProperty]
		private string _textSearch;
		[ObservableProperty]
		private string _weatherType;
		[ObservableProperty]
		private string _sunriseTime;
		[ObservableProperty]
		private string _sunsetTime;
		[ObservableProperty]
		private string _windDirection;
		[ObservableProperty]
		private string _windSpeedUnit;
		[ObservableProperty]
		private string _tempUnit;
        [ObservableProperty]
        private Uri backgroundImageUri;
		[ObservableProperty]
		private Uri weatherIconUri;
		[ObservableProperty]
        private bool isVisible_WeatherData;
		[ObservableProperty]
		private bool isVisible_ConnectionError;

		#endregion Variables

		#region Properties

		public IWeatherService WeatherService { get; set; }
        public IGeoService GeoService { get; set; }
        public IWindowService WindowService  { get; set; }
        public ISettingsService Settings { get; set; }
		public DispatcherTimer SearchDelayTimer { get; set; }
		public DispatcherTimer WeatherUpdateTimer { get; set; }
		public DispatcherTimer ConnectionStatusTimer { get; set; }
        public string ApiKey { get; set; }
        public string WeatherUnitType { get; set; }
        public IPStatus LastIpStatus { get; set; }

        #endregion Properties

        #region Constructors

        public MainWindowViewModel(IWeatherService weatherService, IGeoService geoService, IWindowService windowService, ISettingsService settings) : base()
        {
			WeatherService = weatherService;
            GeoService = geoService;
			WindowService = windowService;
			Settings = settings;

			SyncSettings();

			SearchDelayTimer = new DispatcherTimer();
			SearchDelayTimer.Interval = TimeSpan.FromMilliseconds(500);
			SearchDelayTimer.Tick += SearchDelayTimer_TickAsync;

			WeatherUpdateTimer = new DispatcherTimer();
			WeatherUpdateTimer.Interval = TimeSpan.FromMinutes(5);
			WeatherUpdateTimer.Tick += WeatherUpdateTimer_TickAsync;
			WeatherUpdateTimer.Start();

			ConnectionStatusTimer = new DispatcherTimer();
			ConnectionStatusTimer.Interval = TimeSpan.FromSeconds(3);
			ConnectionStatusTimer.Tick += ConnectionStatusTimer_Tick;
			ConnectionStatusTimer.Start();

			UpdateWeatherDataAsync().Wait();
		}

        #endregion Constructors

        #region Methodes

		private async Task UpdateWeatherDataAsync()
		{
			if(!String.IsNullOrEmpty(ApiKey))
			{
				try
				{
                    WeatherData = await WeatherService.GetWeatherDataAsync(SelectedLocation.Name, SelectedLocation.State, SelectedLocation.Country, WeatherUnitType, ApiKey);
                }
				catch { WeatherData = null; }
				
				if (WeatherData != null)
				{
					SunriseTime = new DateTimeOffset().AddSeconds(WeatherData.Sys.Sunrise + WeatherData.Timezone).ToString("HH:mm");
					SunsetTime = new DateTimeOffset().AddSeconds(WeatherData.Sys.Sunset + WeatherData.Timezone).ToString("HH:mm");
					WindDirection = GetWindDirection(WeatherData.Wind.Deg);
					WeatherType = WeatherData.Weather[0].Main;
					WeatherIconUri = GetWeatherIconUri(WeatherData.Weather[0].Main);
					ChangeBackground(WeatherData.Weather[0].Main);
                    IsVisible_WeatherData = true;
                }
                else
                {
                    WeatherData = new WeatherData();
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/DataError.jpg");
                    IsVisible_WeatherData = false;
                }
            }
		}

        private string GetWindDirection(double degree)
        {
			string direction = "Unknown";

			if ((WeatherData.Wind.Deg >= 0 && WeatherData.Wind.Deg <= 22.5) || (WeatherData.Wind.Deg > 337.5 && WeatherData.Wind.Deg <= 360))
				direction = "Noth";
			else if (WeatherData.Wind.Deg <= 67.5)
				direction = "Noth-East";
			else if (WeatherData.Wind.Deg <= 112.5)
				direction = "East";
			else if (WeatherData.Wind.Deg <= 157.5)
				direction = "South-East";
			else if (WeatherData.Wind.Deg <= 202.5)
				direction = "South";
			else if (WeatherData.Wind.Deg <= 247.5)
				direction = "South-West";
			else if (WeatherData.Wind.Deg <= 292.5)
				direction = "West";
			else if (WeatherData.Wind.Deg <= 337.5)
				direction = "North-West";

			return direction;
		}

		private Uri GetWeatherIconUri(string weatherKey)
		{
			string uri = "pack://application:,,,/Assets/Icons/";

			switch (weatherKey)
			{
				case "Clear":
					uri += "clearSkyDay.png";
					break;
				case "Thunderstorm":
					uri += "thunderstorm.png";
					break;
				case "Drizzle":
					uri += "rainDay.png";
					break;
				case "Rain":
					uri += "rainDay.png";
					break;
				case "Snow":
					uri += "snow.png";
					break;
				case "Mist":
					uri += "mist.png";
					break;
				case "Smoke":
					uri += "mist.png";
					break;
				case "Haze":
					uri += "mist.png";
					break;
				case "Dust":
					uri += "mist.png";
					break;
				case "Fog":
					uri += "mist.png";
					break;
				case "Sand":
					uri += "mist.png";
					break;
				case "Ash":
					uri += "mist.png";
					break;
				case "Squall":
					uri += "mist.png";
					break;
				case "Tornado":
					uri += "mist.png";
					break;
				case "Clouds":
					uri += "scatteredClouds.png";
					break;
				default:
					uri += "question.png";
					break;
			}

			return new Uri(uri);
		}

		private void SyncSettings()
		{
			ApiKey = Settings.Get("apikey");

			SelectedLocation = new GeoData()
			{
				Name = Settings.Get("city"),
				State = Settings.Get("state"),
				Country = Settings.Get("country")
			};

			switch (Settings.Get("unit").ToLower())
			{
				case "standard":
					WeatherUnitType = "standard";
					TempUnit = "K";
					WindSpeedUnit = "m/s";
					break;
				case "imperial":
					WeatherUnitType = "imperial";
					TempUnit = "°F";
					WindSpeedUnit = "mi/h";
					break;
				default:
					WeatherUnitType = "metric";
					TempUnit = "°C";
					WindSpeedUnit = "m/s";
					break;
			}
		}

		private void ChangeBackground(string imageKey)
		{
			switch (imageKey)
			{
				case "Clear":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/clearSky.jpg");
					break;
				case "Thunderstorm":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/thunderstorm.jpg");
					break;
				case "Drizzle":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/rain.jpg");
					break;
				case "Rain":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/showerRain.jpg");
					break;
				case "Snow":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/snow.jpg");
					break;
				case "Mist":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/mist.jpg");
					break;
				case "Smoke":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/smoke.jpg");
					break;
				case "Haze":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/haze.jpg");
					break;
				case "Dust":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/dust.jpg");
					break;
				case "Fog":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/foggy.jpg");
					break;
				case "Sand":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/sandstorm.jpg");
					break;
				case "Ash":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/smoke.jpg");
					break;
				case "Squall":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/squall.jpg");
					break;
				case "Tornado":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/tornado.jpg");
					break;
				case "Clouds":
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/fewClouds.jpg");
					break;
				default:
					BackgroundImageUri = new Uri("pack://application:,,,/Assets/Backgrounds/default.jpg");
					break;

			}    
        }

		#endregion Methodes 

		#region Observable Property Methodes

		async partial void OnTextSearchChanged(string value)
		{
			if (value == String.Empty)
			{
				SearchDelayTimer.Stop();
				IsVisible_GeoDataList = false;
			}
			else
			{
				SearchDelayTimer.Stop();
				SearchDelayTimer.Start();
			}
		}

		#endregion Observable Property Methodes

		#region RelayCommands

		[RelayCommand]
		private void Exit()
		{
			Close();
		}

		[RelayCommand]
		private void OpenHyperLink(string uri)
		{
			Process.Start(new ProcessStartInfo(uri)
			{
				UseShellExecute = true
			});
		}

		[RelayCommand]
		private async Task CitySelected(IGeoData selected)
		{
			if (selected != null)
			{
				SelectedLocation = selected;
				await UpdateWeatherDataAsync();
				TextSearch = String.Empty;

				Settings.Set("city", SelectedLocation.Name);
				Settings.Set("state", SelectedLocation.State);
				Settings.Set("country", SelectedLocation.Country);
			}
		}

		[RelayCommand]
		private async Task OpenSettings()
		{
			var viewModel = App.ServiceProvider.GetRequiredService<SettingsViewModel>();
			var result = WindowService.ShowDialog(viewModel);

			if(result == true)
			{
				SyncSettings();
				await UpdateWeatherDataAsync();
			}
		}

		#endregion RelayCommands

		#region Messages

		#endregion Messages

		#region Events

		async private void SearchDelayTimer_TickAsync(object? sender, EventArgs e)
		{
			SearchDelayTimer.Stop();
			try
			{
                GeoDataList = await GeoService.GetGeoDataAsync(TextSearch, ApiKey);
                IsVisible_GeoDataList = true;
            }
			catch 
			{
                IsVisible_GeoDataList = false;
            }
		}

		async private void WeatherUpdateTimer_TickAsync(object? sender, EventArgs e)
		{
			await UpdateWeatherDataAsync();
		}

		async private void ConnectionStatusTimer_Tick(object? sender, EventArgs e)
		{
			try
			{
				using (Ping pinger = new Ping())
				{
					PingReply reply = pinger.Send(@"openweathermap.org");
					if (reply.Status == IPStatus.Success && LastIpStatus != IPStatus.Success)
					{
						LastIpStatus = IPStatus.Success;
						IsVisible_ConnectionError = false;
						await UpdateWeatherDataAsync();
					}
					else if(reply.Status != IPStatus.Success)
					{ 
						LastIpStatus = reply.Status;
						IsVisible_WeatherData = false;
						IsVisible_ConnectionError = true;
					}
				}
			}
			catch (PingException)
			{
				LastIpStatus = IPStatus.Unknown;
				IsVisible_WeatherData = false;
				IsVisible_ConnectionError = true;
			}
		}

		#endregion Events

		#region Dispose

		#endregion Dispose
	}
}
