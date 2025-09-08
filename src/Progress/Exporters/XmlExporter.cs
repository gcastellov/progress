using Progress.Settings;
using System.Xml;

namespace Progress.Exporters;

internal class XmlExporter : IContentExporter
{
    public FileType FileType => FileType.Xml;

    public string Export(Stats stats)
    {
        var type = stats.GetType();
        string[] properyNames = type.GetProperties().Select(p => p.Name).Order().ToArray();

        XmlDocument xmlDoc = new();
        var root = xmlDoc.CreateElement("Stats");

        foreach (string properyName in properyNames)
        {
            object value = type.GetProperty(properyName)!.GetValue(stats)!;
            var xmlEntry = xmlDoc.CreateElement(properyName);
            xmlEntry.InnerText = value.ToString()!;
            root.AppendChild(xmlEntry);
        }

        xmlDoc.AppendChild(root);
        return xmlDoc.OuterXml;
    }
}
