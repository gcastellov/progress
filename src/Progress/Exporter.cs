using Progress.Exporters;
using Progress.Settings;
using System.Text;

namespace Progress;

/// <summary>
/// Exports the operation result in different formats
/// </summary>
internal class Exporter(ExportSettings settings)
{
    private readonly ExportSettings _settings = settings;
    private readonly IEnumerable<IContentExporter> _exporters = [ new CsvExporter(), new TextExporter(), new JsonExporter(), new XmlExporter() ];

    public void Export(Stats stats)
    {
        using FileStream stream = File.Exists(_settings.FileName)
            ? File.OpenWrite(_settings.FileName)
            : File.Create(_settings.FileName);

        IContentExporter contentExporter = _exporters.FirstOrDefault(e => e.FileType == _settings.FileType) ?? throw new NotSupportedException("Not supported export type");
        string content = contentExporter.Export(stats);
        byte[] result = Encoding.Default.GetBytes(content);
        stream.Write(result, 0, result.Length);
    }
}
