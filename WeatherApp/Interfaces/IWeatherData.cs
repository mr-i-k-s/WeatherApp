using WeatherApp.MVVM.Models

namespace WeatherApp.Interfaces
{
	public interface IWeatherData
	{
		public string Name { get; set; }
		public MainInfo Main { get; set; }
		public List<WeatherInfo> Weather { get; set; }
		public WindInfo Wind { get; set; }
		public SysInfo Sys { get; set; }
		public double Timezone { get; set; }
	}
}
