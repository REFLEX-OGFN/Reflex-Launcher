using IniParser;
using IniParser.Model;
using System;
using System.IO;

namespace WpfApp6.Services
{
    public static class UpdateINI
    {
        public static void WriteToConfig(string SectionName, string PathKey, string NewValue)
        {
            string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string DataFolder = Path.Combine(BaseFolder, "Reflex");
            Directory.CreateDirectory(DataFolder);
            string FilePath = Path.Combine(DataFolder, "Settings.ini");

            FileIniDataParser parser = new FileIniDataParser();

            IniData iniData;
            if (File.Exists(FilePath))
            {
                iniData = parser.ReadFile(FilePath);
            }
            else
            {
                iniData = new IniData();
            }

            iniData[SectionName][PathKey] = NewValue;
            parser.WriteFile(FilePath, iniData);
        }

        public static string ReadValue(string SectionName, string PathKey)
        {
            string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string DataFolder = Path.Combine(BaseFolder, "Reflex");
            string FilePath = Path.Combine(DataFolder, "Settings.ini");

            FileIniDataParser parser = new FileIniDataParser();

            if (File.Exists(FilePath))
            {
                IniData iniData = parser.ReadFile(FilePath);
                return iniData[SectionName][PathKey] ?? "";
            }

            return "";
        }

        public static void DeleteKey(string SectionName, string PathKey)
        {
            string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string DataFolder = Path.Combine(BaseFolder, "Reflex");
            string FilePath = Path.Combine(DataFolder, "Settings.ini");

            if (!File.Exists(FilePath))
            {
                return;
            }

            FileIniDataParser parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile(FilePath);

            if (iniData.Sections.ContainsSection(SectionName) && iniData[SectionName].ContainsKey(PathKey))
            {
                iniData[SectionName].RemoveKey(PathKey);
                parser.WriteFile(FilePath, iniData);
            }
        }
    }
}
