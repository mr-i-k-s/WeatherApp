using Newtonsoft.Json;
using System.IO;
using WeatherApp.Interfaces;

namespace WeatherApp.Services
{
	public class SettingsService : ISettingsService
	{
		public string FileName { get; } = "settings.json";

		public bool Delete(string key)
		{
			throw new NotImplementedException();
		}

		public string Get(string key)
		{
			return GetAll().FirstOrDefault(x => x.Key == key.ToUpper()).Value ?? "";
		}

		public bool Set(string key, string value)
		{
			throw new NotImplementedException();
		}

		private List<KeyValuePair<string, string>> GetAll()
		{
			if (!System.IO.File.Exists(FileName))
			{
				using StreamWriter streamWriter = File.CreateText(FileName);
				streamWriter.WriteLine("[]");
			}

			using StreamReader streamReader =new(FileName);
			string json = streamReader.ReadToEnd();

			var response = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(json);

			return response ?? new List<KeyValuePair<string, string>>();
		}

		private bool Exists(string key)
		{
			return !GetAll().FirstOrDefault(x => x.Key == key.ToUpper()).Equals(default(KeyValuePair<string, string>));
		}

		private KeyValuePair<string, string>? GetObject(string key)
		{
			return GetAll().FirstOrDefault(x => x.Key == key.ToUpper());
		}
	}
}
