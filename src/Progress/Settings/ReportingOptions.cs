namespace Progress.Settings;

internal class ReportingOptions
{
    internal bool DisplayEstimatedTimeOfArrival { get; set; } = true;
    internal bool DisplayRemainingTime { get; set; } = true;
    internal bool DisplayElapsedTime { get; set; } = true;
    internal bool DisplayStartingTime { get; set; } = true;
    internal bool DisplayItemsOverview { get; set; } = true;
    internal bool DisplayItemsSummary { get; set; } = true;
    internal bool NotifyProgressStats { get; set; } = false;
    internal bool NotifyCompletionStats { get; set; } = false;
    internal bool ExportCompletionStats { get; set; } = false;
}
