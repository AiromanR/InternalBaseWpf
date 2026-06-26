using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using InternalBaseWpf.Windows;

namespace InternalBaseWpf.Service
{
    public static class SettingsService
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "InternalBaseWpf",
            "settings.json");

        public static void SaveColumnSettings(string key, List<ColumnSetting> settings)
        {
            var all = LoadAll();
            all[key] = settings;
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(all));
        }

        public static List<ColumnSetting>? LoadColumnSettings(string key)
        {
            var all = LoadAll();
            return all.TryGetValue(key, out var value) ? value : null;
        }

        private static Dictionary<string, List<ColumnSetting>> LoadAll()
        {
            if (!File.Exists(SettingsPath))
                return new Dictionary<string, List<ColumnSetting>>();

            try
            {
                string json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<Dictionary<string, List<ColumnSetting>>>(json)
                       ?? new Dictionary<string, List<ColumnSetting>>();
            }
            catch
            {
                return new Dictionary<string, List<ColumnSetting>>();
            }
        }
    }
}
