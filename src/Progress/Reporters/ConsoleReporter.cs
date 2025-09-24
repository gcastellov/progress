namespace Progress.Reporters;

/// <summary>
/// Reports background task's progress to console based on the settings provided.
/// </summary>
public class ConsoleReporter : ConsoleReporterBase
{
    private Workload Workload => Workloads.ElementAt(0);

    internal ConsoleReporter(Workload workload)
        : base([workload])
    {
    }

    /// <summary>
    /// Reportes success. By calling this method, the success counter increases.
    /// </summary>
    public void ReportSuccess() => Workload.ReportSuccess();

    /// <summary>
    /// Reports failures. By calling this method, the failure counter increases.
    /// </summary>
    public void ReportFailure() => Workload.ReportFailure();

    /// <summary>
    /// Collect stats of the workload
    /// </summary>
    /// <returns></returns>
    protected override Stats CollectStats()
    {
        double percent = Workload.Component.CurrentPercent.Value;

        return new()
        {
            StartedOn = Timer.StartedOn,
            ElapsedTime = Timer.ElapsedTime,
            RemainingTime = Timer.GetRemainingTime(percent),
            EstTimeOfArrival = Timer.GetEstimatedTimeOfArrival(percent),
            ExpectedItems = Workload.ItemsCount,
            CurrentCount = Workload.CurrentCount,
            SuccessCount = Workload.SuccessCount,
            FailureCount = Workload.FailureCount,
            CurrentPercent = percent,
        };
    }
}
