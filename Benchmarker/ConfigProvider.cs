using Benchmarker.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Benchmarker
{
    internal static class ConfigProvider
    {
        public const string Configuration = "configuration.xml";
        public const string GenConfiguration = $"{Configuration}.gen";

        private static bool ContainsConfig(string dirPath)
        {
            var directory = new DirectoryInfo(dirPath);
            return directory.GetFiles(Configuration).Length > 0;
        }

        private static void GenerateConfigAt(string filePath)
        {
            var serializer = new XmlSerializer(typeof(Configuration));
            using var stream = File.Open(filePath, FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, new Configuration());
        }

        public static void GenerateConfig(string dirPath)
        {
            if (!ContainsConfig(dirPath))
            {
                GenerateConfigAt(Path.Combine(dirPath, Configuration));
            }
            GenerateConfigAt(Path.Combine(dirPath, GenConfiguration));
        }

        public static Configuration ReadConfiguration(string dirPath)
        {

            var serializer = new XmlSerializer(typeof(Configuration));
            using var stream = File.Open(Path.Combine(dirPath, Configuration), FileMode.Open, FileAccess.Read);
            var result = serializer.Deserialize(stream) as Configuration;
            return result ?? new Configuration();
        }
    }
}
