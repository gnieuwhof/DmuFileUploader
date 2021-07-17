namespace DmuFileUploader
{
    using Microsoft.VisualBasic.FileIO;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    public static class FileHelper
    {
        public static T DeserializeJsonFile<T>(string file)
        {
            using (StreamReader stream = File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                var result = (T)serializer.Deserialize(stream, typeof(T));

                return result;
            }
        }

        public static void SerializeToJsonFile(object val, string file)
        {
            string json = JsonConvert.SerializeObject(val, Formatting.Indented);

            File.WriteAllText(file, json);
        }

        public static void SerializeXmlFile<T>(string file, T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var writer = new StreamWriter(file))
            {
                xmlSerializer.Serialize(writer, obj);
            }
        }

        public static T DeserializeXmlFile<T>(string file)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (Stream reader = new FileStream(file, FileMode.Open))
            {
                var result = (T)xmlSerializer.Deserialize(reader);

                return result;
            }
        }

        public static string[][] ParseCsvFile(string file)
        {
            var result = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    result.Add(fields);
                }
            }

            return result.ToArray();
        }

        public static string GetWithoutExtension(string file)
        {
            string extension = Path.GetExtension(file);

            if (file.EndsWith(extension))
            {
                return file.Substring(0, file.Length - extension.Length);
            }

            return file;
        }
    }
}
