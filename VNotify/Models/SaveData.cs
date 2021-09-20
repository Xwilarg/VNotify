using System.IO;
using System.Text.Json;

namespace VNotify.Models
{
    public class SaveData
    {
        private SaveData()
        { }

        public static SaveData Load()
        {
            if (_saveData == null)
            {
                if (!File.Exists("data.json")) // No savefile found
                {
                    return new SaveData();
                }
                _saveData = JsonSerializer.Deserialize<SaveData>(File.ReadAllText("data.json")) ?? new SaveData();
            }
            return _saveData;
        }

        public void Save()
        {
            File.WriteAllText("data.json", JsonSerializer.Serialize(this));
        }

        private string? _apiKey;
        public string? ApiKey
        {
            set
            {
                _apiKey = value;
                StaticObjects.HttpClient.DefaultRequestHeaders.Add("X-APIKEY", _apiKey);
            }
            get
            {
                return _apiKey;
            }
        }

        private static SaveData? _saveData;
    }
}
