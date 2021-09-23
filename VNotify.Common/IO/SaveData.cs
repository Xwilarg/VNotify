namespace VNotify.Common.IO
{
    public class SaveData
    {
        public SaveData()
        { }

        public static void Save()
        {
            IOManager<SaveData>.Save(_filename);
        }

        public static SaveData Load()
        {
            return IOManager<SaveData>.Load(_filename);
        }

        const string _filename = "data.json";

        public string ApiKey { set; get; }
    }
}
