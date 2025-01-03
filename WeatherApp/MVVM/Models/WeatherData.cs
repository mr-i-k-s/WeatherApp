using WeatherApp.Interfaces;

namespace WeatherApp.MVVM.Models
{
	public class WeatherData : IWeatherData
	{
        public string Name { get; set; }
		public MainInfo Main { get; set; }
		public List<WeatherInfo> Weather { get; set; }
        public WindInfo Wind { get; set; }
		public SysInfo Sys { get; set; }
        public double Timezone { get; set; }
    }

	public class WeatherInfo : IWeatherInfo
	{
        public string Main { get; set; }
    }

	public class MainInfo : IMainInfo
	{
		public float Temp { get; set; }
		public float Humidity { get; set; }
	}

	public class SysInfo : ISysInfo
	{
        public double Sunrise { get; set; }
		public double Sunset { get; set; }
	}

	public class WindInfo : IWindInfo
	{
		public float Speed { get; set; }
		public float Deg { get; set; }
		public float Gust { get; set; }
	}
}
