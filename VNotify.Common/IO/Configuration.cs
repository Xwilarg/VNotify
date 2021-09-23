namespace VNotify.Common.IO
{
    public class Configuration
    {
        public Configuration()
        { }

        public static void Save()
        {
            IOManager<Configuration>.Save(_filename);
        }

        public static Configuration Load()
        {
            return IOManager<Configuration>.Load(_filename);
        }

        const string _filename = "config.json";

        public string ApiKey { set; get; }
    }
}
