namespace WeatherApp.Interfaces
{
	public interface IWindowService
	{
		void ShowWindow(object viewModel);
		bool? ShowDialog(object viewModel);
	}
}
