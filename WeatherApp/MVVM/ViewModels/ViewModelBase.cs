using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
