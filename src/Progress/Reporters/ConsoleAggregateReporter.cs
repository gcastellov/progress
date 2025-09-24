namespace Progress.Reporters;

/// <summary>
/// Reports many background task's progress to console based on the settings provided.
/// </summary>
public class ConsoleAggregateReporter : ConsoleReporterBase
{
    internal ConsoleAggregateReporter(ICollection<Workload> workloads)
        : base(workloads)
    {
    }

    /// <summary>
    /// Reportes success. By calling this method, the success counter increases.
    /// </summary>
    public void ReportSuccess(string workloadName) => Workloads.Success(workloadName);

    /// <summary>
    /// Reports failures. By calling this method, the failure counter increases.
    /// </summary>
    public void ReportFailure(string workloadName) => Workloads.Failure(workloadName);

    /// <summary>
    /// Collect stats relative to all workloads
    /// </summary>
    /// <returns></returns>
    protected override Stats CollectStats()
    {
        double currentPercent = Workloads.CurrentPercent;

        return new()
        {
            StartedOn = Timer.StartedOn,
            ElapsedTime = Timer.ElapsedTime,
            RemainingTime = Timer.GetRemainingTime(currentPercent),
            EstTimeOfArrival = Timer.GetEstimatedTimeOfArrival(currentPercent),
            ExpectedItems = Workloads.AllItemsCount,
            SuccessCount = Workloads.AllSuccess,
            FailureCount = Workloads.AllFailures,
            CurrentCount = Workloads.CurrentCount,
            CurrentPercent = Workloads.CurrentPercent,
        };
    }
}
