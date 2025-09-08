using Progress.Settings;
using System.Text.Json;

namespace Progress.Exporters
{
    internal class JsonExporter : IContentExporter
    {
        public FileType FileType => FileType.Json;

        public string Export(Stats stats) => JsonSerializer.Serialize(stats);
    }
}
