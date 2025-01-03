using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface IWindowService
	{
		void ShowWindow(object viewModel);
		bool? ShowDialog(object viewModel);
	}
}
