using Progress.Settings;
using System.Text;

namespace Progress.Exporters
{
    internal class TextExporter : IContentExporter
    {
        private const int LeftPadding = 30;
        private const int RightPadding = 20;

        public FileType FileType => FileType.Text;

        public string Export(Stats stats)
        {
            var type = stats.GetType();
            string[] properyNames = type.GetProperties().Select(p => p.Name).Order().ToArray();

            StringBuilder sBuilder = new();

            foreach (string properyName in properyNames)
            {
                object value = type.GetProperty(properyName)!.GetValue(stats)!;
                sBuilder.Append($"{properyName}:".PadRight(RightPadding));
                sBuilder.Append(value.ToString()!.PadLeft(LeftPadding));
                sBuilder.AppendLine();
            }

            return sBuilder.ToString();
        }
    }
}
