using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows.Controls;
using System.Windows.Threading;
using WeatherApp.Interfaces;
using WeatherApp.MVVM.Models;
using WeatherApp.Services;

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
		private bool _isVisibleGeoDataList;
		[ObservableProperty]
		private IGeoData? _selectedLocation;
		[ObservableProperty]
		private string _searchText;
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
        private bool isVisible_WeatherData;

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

        #endregion Properties

        #region Constructors

        public MainWindowViewModel(IWeatherService weatherService, IGeoService geoService, IWindowService windowService, ISettingsService settings)
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
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/clear sky.jpg");
                    break;
                case "Clouds":
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/broken clouds.jpg");
                    break;
                case "Rain":
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/shower rain.jpg");
                    break;
                case "Drizzle":
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/rain.jpg");
                    break;
                case "Thunderstorm":
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/thunderstorm.jpg");
                    break;
                case "Snow":
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/snow.jpg");
                    break;
                case "Mist":
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/mist.jpg");
                    break;
                default:
                    BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/neutral.jpg");
                    break;

            }    
        }

		#endregion Methodes 

		#region Observable Property Methodes

		async partial void OnSearchTextChanged(string value)
		{
			if (value == String.Empty)
			{
				SearchDelayTimer.Stop();
				IsVisibleGeoDataList = false;
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
				SearchText = String.Empty;

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
                GeoDataList = await GeoService.GetGeoDataAsync(SearchText, ApiKey);
                IsVisibleGeoDataList = true;
            }
			catch 
			{
                IsVisibleGeoDataList = false;
            }
		}

		async private void WeatherUpdateTimer_TickAsync(object? sender, EventArgs e)
		{
			await UpdateWeatherDataAsync();
		}

		async private void ConnectionStatusTimer_Tick(object? sender, EventArgs e)
		{
			//var weatherConnectionState = MyWeatherService.CheckConnection();
			//var geoConnectionState = MyGeoService.CheckConnection();

			//if (weatherConnectionState == IPStatus.Success && geoConnectionState == IPStatus.Success)
			//{
			//	ConnectionImageUri = new Uri("pack://application:,,,/Assets/Images/Background/satelliteGreen.png");
			//	if (LastConnectionStatus != IPStatus.Success)
			//	{
			//		LastConnectionStatus = IPStatus.Success;
			//		await UpdateWeatherDataAsync();
			//	}
			//}
			//else
			//{
			//	LastConnectionStatus = IPStatus.Unknown;
			//	IsVisible_WeatherData = false;
			//	BackgroundImageUri = new Uri("pack://application:,,,/Assets/Images/Background/DataError.jpg");
			//	ConnectionImageUri = new Uri("pack://application:,,,/Assets/Images/Background/satelliteRed.png");
			//}
		}

		#endregion Events

		#region Dispose

		#endregion Dispose
	}
}
