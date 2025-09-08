using Progress.Settings;

namespace Progress.Exporters
{
    internal interface IContentExporter
    {
        FileType FileType { get; }
        string Export(Stats stats);
    }
}
