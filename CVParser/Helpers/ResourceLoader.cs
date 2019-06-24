using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CVParser.Helpers
{
    public static class ResourceLoader
    {
        public static HashSet<string> LoadIntoHashSet(string fileName, char delimiter, Encoding encoding)
        {
            var lines = Load(fileName, delimiter, encoding);
            return new HashSet<string>(lines, StringComparer.InvariantCultureIgnoreCase);
        }

        public static List<string> LoadIntoList(string fileName, char delimiter, Encoding encoding)
        {
            var lines = Load(fileName, delimiter, encoding);
            return new List<string>(lines);
        }

        private static IEnumerable<string> Load(string fileName, char delimiter, Encoding encoding)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "wwwroot", "CV Parser", "Data", fileName);
            string content = File.ReadAllText(path, encoding);

            return content.Split(delimiter);
        }
    }
}
