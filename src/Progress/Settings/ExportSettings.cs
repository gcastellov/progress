namespace Progress.Settings;

/// <summary>
/// Supported export file types
/// </summary>
public enum FileType
{
    /// <summary>
    /// CSV
    /// </summary>
    Csv,
    /// <summary>
    /// TXT
    /// </summary>
    Text,
    /// <summary>
    /// JSON
    /// </summary>
    Json,
    /// <summary>
    /// XML
    /// </summary>
    Xml
}

/// <summary>
/// Define the export settings
/// </summary>
/// <param name="FileName"></param>
/// <param name="FileType"></param>
public record ExportSettings(string FileName, FileType FileType);