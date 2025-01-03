using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;
using WeatherApp.Interfaces;
using WeatherApp.Services;

namespace WeatherApp.MVVM.ViewModels
{
	public partial class SettingsViewModel : ViewModelBase
	{
        #region Variables

        [ObservableProperty]
        private string _apiKey;
		[ObservableProperty]
		private bool isChecked_Metric;
		[ObservableProperty]
		private bool isChecked_Imperial;
		[ObservableProperty]
		private bool isChecked_Standard;

		#endregion Variables

		#region Properties

		public ISettingsService SettingsService { get; set; }

        #endregion Properties

        #region Constructors

        public SettingsViewModel(ISettingsService settingsService)
        {
			SettingsService = settingsService;

            ApiKey = SettingsService.Get("apikey");

			switch (SettingsService.Get("unit").ToLower())
			{
				case "standard":
					IsChecked_Standard = true;
					break;
				case "imperial":
					IsChecked_Imperial = true;
					break;
				default:
					IsChecked_Metric = true;
					break;
			}
		}

		#endregion Constructors

		#region Methodes

		#endregion Methodes 

		#region Observable Property Methodes

		#endregion Observable Property Methodes

		#region RelayCommands

		[RelayCommand]
		private void Save()
		{
			SettingsService.Set("apikey", ApiKey);

			if (IsChecked_Metric) SettingsService.Set("unit", "metric");
			else if (IsChecked_Imperial) SettingsService.Set("unit", "imperial");
			else SettingsService.Set("unit", "standard");

			Close(true);
		}


		[RelayCommand]
		private void CreateNewApi(string uri)
		{
			Process.Start(new ProcessStartInfo(uri)
			{
				UseShellExecute = true
			});
		}

		#endregion RelayCommands

		#region Messages

		#endregion Messages

		#region Events

		#endregion Events

		#region Dispose

		#endregion Dispose
	}
}
