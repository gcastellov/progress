namespace Progress.Settings.Background;

internal class Configuration
{
    public ReportingOptions Options { get; init; } = new();
    public ExportSettings ExportSettings { get; set; } = default!;
    public TimeSpan StatsFrequency { get; set; } = TimeSpan.FromSeconds(5);
}
