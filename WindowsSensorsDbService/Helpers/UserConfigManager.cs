using System.Xml.Linq;

namespace WindowsSensorsDbService.Helpers
{
    public static class UserConfigManager
    {
        private static readonly string ConfigFilePath = "/tmp/Config.xml";

        public static void CreateConfigFile()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilePath));

                XDocument doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("config",
                        new XElement("settings")));

                doc.Save(ConfigFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating settings file: {ex.Message}");
            }
        }

        public static string GetSetting(string settingName, string defaultValue = "")
        {
            if (!File.Exists(ConfigFilePath))
            {
                CreateConfigFile();
            }

            try
            {
                XDocument doc = XDocument.Load(ConfigFilePath);
                var setting = doc.Descendants("setting")
                                 .FirstOrDefault(s => s.Attribute("name").Value == settingName);

                if (setting != null)
                {
                    return setting.Attribute("value").Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }

            return defaultValue;
        }

        public static void SetSetting(string settingName, string value)
        {
            try
            {
                XDocument doc = XDocument.Load(ConfigFilePath);
                var setting = doc.Descendants("setting")
                                 .FirstOrDefault(s => s.Attribute("name").Value == settingName);

                if (setting != null)
                {
                    setting.Attribute("value").Value = value;
                }
                else
                {
                    doc.Root.Element("settings").Add(new XElement("setting",
                        new XAttribute("name", settingName),
                        new XAttribute("value", value)));
                }

                doc.Save(ConfigFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}
