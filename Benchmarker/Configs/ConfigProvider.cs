using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Benchmarker.Configs
{
    internal static class ConfigProvider
    {
        public const string ConfName = "configuration.xml";
        public const string GenConfName = $"{ConfName}.gen";

        private static bool ContainsConfig(string dirPath)
        {
            var directory = new DirectoryInfo(dirPath);
            return directory.GetFiles(ConfName).Length > 0;
        }

        private static void GenerateConfigAt(string filePath)
        {
            var config = Configuration.Default;
            var serializer = new XmlSerializer(typeof(Configuration));
            using var stream = File.Open(filePath, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, config);
        }

        public static void GenerateConfig(string dirPath)
        {
            if (!ContainsConfig(dirPath))
            {
                GenerateConfigAt(Path.Combine(dirPath, ConfName));
            }
            GenerateConfigAt(Path.Combine(dirPath, GenConfName));
        }

        public static Configuration ReadConfiguration(string dirPath)
        {

            var serializer = new XmlSerializer(typeof(Configuration));
            using var stream = File.Open(Path.Combine(dirPath, ConfName), FileMode.Open, FileAccess.Read);
            var result = serializer.Deserialize(stream) as Configuration;
            return result ?? Configuration.Default;
        }
    }
}
