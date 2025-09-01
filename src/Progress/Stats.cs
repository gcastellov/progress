namespace Progress;

/// <summary>
/// Represents statisitics about the operation
/// </summary>
public class Stats
{
    /// <summary>
    /// Gets the starting time
    /// </summary>
    public DateTimeOffset StartedOn { get; internal init; }

    /// <summary>
    /// Gets the ETA
    /// </summary>
    public DateTimeOffset EstTimeOfArrival { get; internal init; }

    /// <summary>
    /// Gets the elapsed time
    /// </summary>
    public TimeSpan ElapsedTime { get; internal init; }

    /// <summary>
    /// Gets the remaining time
    /// </summary>
    public TimeSpan RemainingTime { get; internal init; }

    /// <summary>
    /// Gets the count of success
    /// </summary>
    public ulong SuccessCount { get; internal init; }

    /// <summary>
    /// Gets the count of failures
    /// </summary>
    public ulong FailureCount { get; internal init; }

    /// <summary>
    /// Gets the current count of failures and success
    /// </summary>
    public ulong CurrentCount { get; internal init; }

    /// <summary>
    /// Gets the expected items to process
    /// </summary>
    public ulong ExpectedItems {  get; internal init; }

    /// <summary>
    /// Gets the current percent
    /// </summary>
    public double CurrentPercent { get; internal init; }
}
