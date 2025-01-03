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
			if (Exists(key))
			{
				var _data = GetAll().Where(x => x.Key != key.ToUpper()).ToArray();
				string json = JsonConvert.SerializeObject(_data, Formatting.Indented);

				//write string to destination
				File.WriteAllText(FileName, json);

				return true;
			}
			return false;
		}

		public string Get(string key)
		{
			return GetAll().FirstOrDefault(x => x.Key == key.ToUpper()).Value ?? "";
		}

		public bool Set(string key, string value)
		{
			key = key.ToUpper();
			var Item = GetObject(key);
			var _data = GetAll().ToList();

			if (Item.Equals(default(KeyValuePair<string, string>)))
			{
				//Add new key in Data Items
				_data.Add(new KeyValuePair<string, string>(key, value));
			}
			else
			{
				_data.Remove(_data.First(x => x.Key == key)); //Remove old entry
				_data.Add(new KeyValuePair<string, string>(key, value)); //add updated entry
			}

			string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented);
			//write string to destination
			File.WriteAllText(FileName, json);

			return true;
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
