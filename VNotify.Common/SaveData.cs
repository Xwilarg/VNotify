using System;
using System.IO;
using System.Text.Json;

namespace VNotify.Common
{
    public class SaveData
    {
        public SaveData()
        { }

        public static SaveData Load()
        {
            if (_saveData == null)
            {
                if (!Directory.Exists(_dataPath))
                {
                    Directory.CreateDirectory(_dataPath);
                }
                if (!File.Exists(_dataPath + "data.json")) // No savefile found
                {
                    return new SaveData();
                }
                _saveData = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(_dataPath + "data.json")) ?? new SaveData();
            }
            return _saveData;
        }

        public void Save()
        {
            File.WriteAllText(_dataPath + "data.json", JsonSerializer.Serialize(this));
        }

        public string ApiKey { set; get; }

        private static SaveData _saveData;

        private static string _dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).Replace('\\', '/') + "/VNotify/";
    }
}
