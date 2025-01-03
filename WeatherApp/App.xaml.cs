using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WeatherApp.Interfaces;
using WeatherApp.MVVM.ViewModels;
using WeatherApp.MVVM.Views;
using WeatherApp.Services;

namespace WeatherApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static IServiceProvider ServiceProvider { get; private set; }

		public App()
		{
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			ServiceProvider = serviceCollection.BuildServiceProvider();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			var windowService = ServiceProvider.GetRequiredService<IWindowService>();
			var mainWindowViewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
			windowService.ShowWindow(mainWindowViewModel);
		}

		private void ConfigureServices(IServiceCollection services)
		{
			// Configure Logging
			services.AddLogging();

			// Register Services
			services.AddSingleton<IWindowService, WindowService>();
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<IWeatherService, WeatherService>();
			services.AddSingleton<IGeoService, GeoService>();

			// Register ViewModels
			services.AddTransient<MainWindowViewModel>();
			services.AddTransient<SettingsViewModel>();

			// Register Views
			services.AddSingleton<MainWindowView>();

		}
	}

}
