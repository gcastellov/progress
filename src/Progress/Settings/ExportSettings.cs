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

internal record ExportSettings(string FileName, FileType FileType);