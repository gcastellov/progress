namespace Progress.Settings;

internal class Configuration
{
    public ReportingOptions Options { get; init; } = new();
    public ExportSettings ExportSettings { get; set; } = default!;
    public TimeSpan ReportFrequency { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan StatsFrequency { get; set; } = TimeSpan.FromSeconds(5);
}
