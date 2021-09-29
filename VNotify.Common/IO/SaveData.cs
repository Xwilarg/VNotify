using System;
using System.Collections.Generic;
using System.Linq;

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

        private List<string> _subscriptions;
        public List<string> Subscriptions
        {
            set
            {
                _subscriptions = value;
            }
            get
            {
                if (_subscriptions == null)
                {
                    return Array.Empty<string>().ToList();
                }
                return _subscriptions;
            }
        }
    }
}
