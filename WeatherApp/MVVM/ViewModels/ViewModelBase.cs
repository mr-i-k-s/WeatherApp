using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using WeatherApp.Interfaces;

namespace WeatherApp.MVVM.ViewModels
{
	public class ViewModelBase : ObservableValidator, IViewAware, IDisposable
	{
		public Window View { get; set; }

		public void Close()
		{
			View.Close();
		}

		public void Close(bool? result)
		{
			View.DialogResult = result;
			View.Close();
		}

		public virtual void Dispose()
		{

		}
	}
}
