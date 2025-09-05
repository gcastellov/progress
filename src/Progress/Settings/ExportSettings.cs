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
    Json
}

internal record ExportSettings(string FileName, FileType FileType);