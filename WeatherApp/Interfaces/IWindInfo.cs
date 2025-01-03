using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Interfaces
{
	public interface IWindInfo
	{
		public float Speed { get; set; }
		public float Deg { get; set; }
		public float Gust { get; set; }
	}
}
