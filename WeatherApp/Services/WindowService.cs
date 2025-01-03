using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherApp.Interfaces;

namespace WeatherApp.Services
{
	public class WindowService : IWindowService
	{
		public bool? ShowDialog(object viewModel)
		{
			throw new NotImplementedException();
		}

		public void ShowWindow(object viewModel)
		{
			throw new NotImplementedException();
		}

		private Window CreateWindow(object viewModel, bool isDialog)
		{
			var window = FindWindowInAssebly(viewModel);
			var view = SetUpWindow(window, isDialog);

			return view;
		}
	}
}
