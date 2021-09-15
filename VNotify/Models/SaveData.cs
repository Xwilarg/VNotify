using System.IO;
using System.Text.Json;

namespace VNotify.Models
{
    public class SaveData
    {
        public SaveData()
        { }

        public static SaveData Load()
        {
            if (!File.Exists("data.json")) // No savefile found
            {
                return new SaveData();
            }
            return JsonSerializer.Deserialize<SaveData>(File.ReadAllText("data.json")) ?? new SaveData();
        }

        public string ApiKey { set; get; }
    }
}
