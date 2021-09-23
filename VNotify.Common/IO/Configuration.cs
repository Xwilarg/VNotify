using System;

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

        private Channel[] _channels;
        public Channel[] Channels
        {
            set => _channels = value;
            get
            {
                return _channels ?? Array.Empty<Channel>();
            }
        }
    }
}
