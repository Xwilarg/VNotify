using System.IO;
using System.Text.Json;

namespace VNotify.Application.Models
{
    public class SaveData
    {
        public SaveData()
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

        public string? ApiKey;

        private static SaveData? _saveData;
    }
}
