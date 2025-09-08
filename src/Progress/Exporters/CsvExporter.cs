using Progress.Settings;
using System.Text;

namespace Progress.Exporters
{
    internal class CsvExporter : IContentExporter
    {
        private const char Separator = ';'; 

        public FileType FileType => FileType.Csv;

        public string Export(Stats stats)
        {
            var type = stats.GetType();
            string[] properyNames = type.GetProperties().Select(p => p.Name).Order().ToArray();

            StringBuilder sBuilder = new();
            sBuilder.AppendLine(string.Join(Separator, properyNames));

            foreach(string properyName in properyNames)
            {
                object value = type.GetProperty(properyName)!.GetValue(stats)!;
                sBuilder.Append(value.ToString());
                sBuilder.Append(Separator);
            }
            
            return sBuilder.ToString();
        }
    }
}
