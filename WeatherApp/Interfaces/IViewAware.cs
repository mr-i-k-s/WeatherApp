using System.Windows;

namespace WeatherApp.Interfaces
{
	public interface IViewAware
	{
		Window View { get; set; }
	}
}
