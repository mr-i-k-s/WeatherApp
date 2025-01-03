using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherApp.Interfaces
{
	public interface IViewAware
	{
		Window View { get; set; }
	}
}
