using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface IMainInfo
	{
		public float Temp { get; set; }
		public float Humidity { get; set; }
	}
}
