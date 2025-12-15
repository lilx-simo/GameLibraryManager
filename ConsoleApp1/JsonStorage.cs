using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GameLibraryManager
{
    public static class JsonStorage
    {
        public static List<T> LoadList<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<T>();
                }

                var json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<T>();
                }

                var data = JsonConvert.DeserializeObject<List<T>>(json);
                return data ?? new List<T>();
            }
            catch (Exception)
            {
                // If file is corrupt or invalid, return empty list.
                return new List<T>();
            }
        }

        public static void SaveList<T>(string filePath, List<T> items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}



