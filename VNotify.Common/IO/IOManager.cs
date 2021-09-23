using System;
using System.IO;
using System.Text.Json;

namespace VNotify.Common.IO
{
    public class IOManager<T>
        where T : new()
    {
        internal static void Save(string filename)
        {
            File.WriteAllText(_dataPath + filename, JsonSerializer.Serialize(_data));
        }

        internal static T Load(string filename)
        {
            if (_data == null)
            {
                if (!Directory.Exists(_dataPath))
                {
                    Directory.CreateDirectory(_dataPath);
                }
                if (!File.Exists(_dataPath + filename)) // No savefile found
                {
                    return new T();
                }
                _data = JsonSerializer.Deserialize<T>(File.ReadAllText(_dataPath + filename)) ?? new T();
            }
            return _data;
        }

        private static T _data;
        private static readonly string _dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).Replace('\\', '/') + "/VNotify/";
    }
}
